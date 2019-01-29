using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EntityPlayer : MonoBehaviour, IDamageable {
	private enum DIRECTION {
		FRONT = 0,
		BACK,
		RIGHT,
		LEFT
	}

	[SerializeField]
	private SOList
	_ElementType;

	[SerializeField]
	private GameObject
	_SuckingParticle,
	_SuckingRadius,
	_WalkParticle;

	[SerializeField]
	private uint
	_Orb_Slots,
	_Skill_Slots;

	[SerializeField]
	private GameObject
	_PrefabInstance;

	// prediction manager
	[SerializeField]
	private PredictionManager _preMan;

	public Stats
	_Player_Stats;

	private float
	_Money;

	private uint
	_Casting_Animation_ID;

	private bool
	_RestrictMovement,
	_Cast_Trigger,
	_Is_Casting,
	_Is_VacuumSFXPlayed;

	private Status
	_Status;

	[SerializeField]
	private SOList
	_skillTier,
	_baseSkill,
	_combiSkill;

	private List<ElementType>
		_OrbSlot = new List<ElementType>();

	private Dictionary<ElementType, int>
		_SlimeStock = new Dictionary<ElementType, int>();

	private Queue<ElementType>
		_tmpStock = new Queue<ElementType>();

	private List<Skill>
		_Skills = new List<Skill>();

	private Skill
	_CurrentSkillOutcome,
	_CurrentUseSkill;

	private Vector3
	_PrevPosition;

	private int
	_CurrentSelection;

	private int
	_OldSelection;

	private EnumHolder.States
	_Player_State;

	private DIRECTION
	_Player_Dir;

    private Dictionary<EnumHolder.States, CheckFunctions>
		_CheckFuntions = new Dictionary<EnumHolder.States, CheckFunctions>();
	private bool _controllable = true;
	// プレイヤーが操作可能か？
	public bool Controllable {
		get {
			return _controllable;
		}
		set {
			_controllable = value;
		}
	}

	// 無敵時間
	[SerializeField, Range(0.0f, 3.0f)]
	private float _invincibleTime = 3.0f;

	private bool _isInvincible = false;

    public Dictionary<ElementType, int> SlimeStock { get { return _SlimeStock; } }

	public float MaxHitPoint { get { return _Player_Stats.MaxHealthProperties * _Player_Stats.HealthMultiplyerProperties; } }
	public float HitPoint { get { return _Player_Stats.HealthProperties; } }
	public float MoneyAmount { get { return _Money; } set{_Money = value;}}
	public float Speed {
		get {
			return _Player_Stats.SpeedProperties * _Player_Stats.SpeedMultiplyerProperties;
		}
	}

	private Animator
	_Animator;

	public bool RestrictMovement {
		get {
			return _RestrictMovement;
		}

		set {
			_RestrictMovement = value;
		}
	}

	[SerializeField]
	private GameObject
	_CastingPoint;
    private void Start(){
        _Player_Stats.IsInvincible = false;

        // プレイヤーのHPが減少したら
        this.ObserveEveryValueChanged(_ => this.HitPoint)
            .Buffer(2, 1)
            .Where(x => x.Count == 2)
            .Select(x => x.First() - x.Last())
            .Where(x => x > 0)
            .Subscribe(x =>
            {
                _Player_Stats.IsInvincible = true;
                if (_isInvincible)
                {
                    StopCoroutine(InvincibleTimer());
                }
                StartCoroutine(InvincibleTimer());
            });

        // プレイヤー死亡時、スロットを空にする
        this.ObserveEveryValueChanged(_ => this._Player_State)
            .Where(x => x == EnumHolder.States.DIE)
            .Subscribe(x =>
            {
                _OrbSlot.Clear();
                _Skills.Clear();
            });
    }

	IEnumerator InvincibleTimer() {
		_isInvincible = true;
		yield return new WaitForSeconds(_invincibleTime);
		_Player_Stats.IsInvincible = false;
		_isInvincible = false;
	}

	[SerializeField]
	private AudioClip
	_SkillStoreSFX,
	_VacuumSFX,
	_SuckSFX,
	_WalkingSFX,
	_SkillSwapSFX;

	private void Awake() {
		_CurrentSkillOutcome = null;
		_CurrentSelection = 0;
		_Cast_Trigger = false;
		_Is_Casting = false;
		_Is_VacuumSFXPlayed = false;

		_Player_Stats = EnumHolder.Instance.GetStats(gameObject.name);

		_Player_Stats.DamageMultiplyerProperties = _Player_Stats.HealthMultiplyerProperties = _Player_Stats.SpeedMultiplyerProperties = _Player_Stats.SuckingPowerMultiplyerProperties = 1;
		_Player_Stats.HealthProperties = MaxHitPoint;

		_Player_Stats.HealthMultiplyerProperties = 1.0f;
		_Player_Stats.SkillPowerMultiplyerProperties = 1.0f;

		_Money = 0;

		_Player_State = EnumHolder.States.IDLE;
		_Player_Dir = DIRECTION.FRONT;

		_Animator = gameObject.GetComponentInChildren<Animator>();
		_Status = gameObject.GetComponent<Status>();
		_Status.Init();

		_CheckFuntions.Add(EnumHolder.States.IDLE, IdleCheckFunction);
		_CheckFuntions.Add(EnumHolder.States.MOVING, MovingCheckFunction);
		_CheckFuntions.Add(EnumHolder.States.KICKING, KickingCheckFunction);
		_CheckFuntions.Add(EnumHolder.States.DIE, DieCheckFunction);

		_SuckingParticle.SetActive(false);
		_SuckingRadius.SetActive(false);
		_Animator.SetBool("IsSucking", false);

		_Skills.Capacity = (int) _Skill_Slots;
		for (int i = 0; i < _Skill_Slots; i++) {
			_Skills.Add(null);
		}
	}

	private void IdleCheckFunction() {
		_PrevPosition = gameObject.transform.position;

		//if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		if (InputManager.LS_Joystick() != Vector3.zero) {
			_Player_Dir = DIRECTION.FRONT;
			_Player_State = EnumHolder.States.MOVING;
			return;
		}
	}

	private void MovingCheckFunction() {
		//if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
		if (InputManager.LS_Joystick() == Vector3.zero) {
			_Player_State = EnumHolder.States.IDLE;
			return;
		}

		float ForwardAngle = Vector3.Angle(gameObject.transform.forward.normalized, (_PrevPosition - gameObject.transform.position).normalized);
		float RightAngle = Vector3.Angle(gameObject.transform.right.normalized, (_PrevPosition - gameObject.transform.position).normalized);

		if (ForwardAngle < 30)
			_Player_Dir = DIRECTION.BACK;
		else if (ForwardAngle > 150)
			_Player_Dir = DIRECTION.FRONT;
		else if (ForwardAngle >= 30 && ForwardAngle <= 150) {
			if (RightAngle < 90)
				_Player_Dir = DIRECTION.RIGHT;
			else
				_Player_Dir = DIRECTION.LEFT;
		}

		_PrevPosition = gameObject.transform.position;
	}

	private void KickingCheckFunction() {

	}

	private void CastingCheckFunction() {
		if (_Player_State == EnumHolder.States.IDLE) {
			_Animator.SetLayerWeight(3, 1f);
			_Animator.SetLayerWeight(1, 0.5f);
		} else {
			_Animator.SetLayerWeight(3, 0.5f);
			_Animator.SetLayerWeight(1, 1f);
		}

		if (!_Animator.GetBool("IsCasting"))
			_Animator.SetBool("IsCasting", true);

		_CurrentUseSkill.Engage(gameObject, _CastingPoint.transform.position, gameObject.transform.forward.normalized);

		if (_Casting_Animation_ID != _CurrentUseSkill.GetAnimationID()) {
			_Casting_Animation_ID = _CurrentUseSkill.GetAnimationID();
			_Animator.SetInteger("CastingID", (int) _Casting_Animation_ID);
		}

		if (RestrictMovement != _CurrentUseSkill.IsMoveOnCast())
			RestrictMovement = _CurrentUseSkill.IsMoveOnCast();

		if (_CurrentUseSkill.IsSkillOver() && _CurrentUseSkill.IsTimeOver()) {
			ResetCurrentUsedSkill();
			if (_Animator.GetBool("IsCasting")) {
				_Animator.SetBool("IsCasting", false);
				_Animator.SetLayerWeight(3, 1f);
				_Animator.SetLayerWeight(1, 1f);
			}
		}
	}

	private void DieCheckFunction() {
		if (!IsDead())
			_Player_State = EnumHolder.States.IDLE;
	}

	// Update is called once per frame
	private void Update() {
		_Status.UpdateStatMultiplyer(ref _Player_Stats);
		TakeDamage(_Status.GetValue(EnumHolder.EffectType.TAKEDAMAGE));

		_CheckFuntions[_Player_State].Invoke();
		_Animator.SetInteger("State", (int) _Player_State);
		_Animator.SetInteger("Direction", (int) _Player_Dir);

		if (!_Is_Casting &&
			_Player_State != EnumHolder.States.KICKING &&
			_Player_State != EnumHolder.States.DIE &&
			Speed > 0) {
			if (InputManager.Suck_Input()) {
				StartSuck();
				_Animator.SetBool("IsSucking", true);

				if (!_Is_VacuumSFXPlayed) {
					_Is_VacuumSFXPlayed = true;
					AudioManager.Instance.PlaySEWithFadeIn(_VacuumSFX.name, 0.5f, true);
				}

			} else {
				if (_Is_VacuumSFXPlayed) {
					_Is_VacuumSFXPlayed = false;
					AudioManager.Instance.StopSE(_VacuumSFX.name);
				}

				if (_SuckingParticle.activeSelf)
					_SuckingParticle.SetActive(false);

				if (_SuckingRadius.activeSelf)
					_SuckingRadius.SetActive(false);

				if (_Animator.GetBool("IsSucking"))
					_Animator.SetBool("IsSucking", false);
			}

			//Reset
			if (Input.GetKeyDown(KeyCode.LeftControl)) {
				ResetOrbSlots();
				_tmpStock.Clear();
			}

			//Storing of skill
			if (InputManager.CombineOrbs_Input()) {
				StoreSkills();
				_preMan.SwitchMode(_Skills[_CurrentSelection]);

				// スライムストックから使ったスライムを抜く
				foreach (var type in _tmpStock) {
					_SlimeStock[type]--;
				}
				_tmpStock.Clear();
			}

			if (_Skills.Count > 0) {
				if (_Skills.Count > 1) {
					float position = InputManager.SkillScroll_Input();
					if (position > 0f) {
						AudioManager.Instance.PlaySE(_SkillSwapSFX.name);
						--_CurrentSelection;
					} else if (position < 0f) {
						AudioManager.Instance.PlaySE(_SkillSwapSFX.name);
						++_CurrentSelection;
					}
				} else
					_CurrentSelection = 0;

				if (InputManager.UseSkills_Input()) {
					if (!_Cast_Trigger) {
						UseSkill();
						_Cast_Trigger = true;
						if (_Skills.Count > 0)
							_preMan.SwitchMode(_Skills[_CurrentSelection]);
					}
				} else
					_Cast_Trigger = false;
			}

		}

		if (_Is_Casting) {
			CastingCheckFunction();
		}

		if (IsDead() && _Player_State != EnumHolder.States.DIE) {
			if (_Is_Casting)
				ResetCurrentUsedSkill();
			_Player_State = EnumHolder.States.DIE;
			GameStateManager.Instance.PlayerDead();
		}

		if (_CurrentSelection < 0)
			_CurrentSelection = _Skills.Count - 1;
		else if (_CurrentSelection > _Skills.Count - 1)
			_CurrentSelection = 0;

		// change prediction line by current skill
		if (_Skills.Count > 0) {
			if (_CurrentSelection != _OldSelection)
				_preMan.SwitchMode(_Skills[_CurrentSelection]);
			if (_Skills[_CurrentSelection] != null)
				_Skills[_CurrentSelection].GetSkillTier();
		} else {
			_preMan.SwitchMode(null);
		}

		//// テキストコンポーネントを取得し、スライムの数を入れる
		//foreach (var slime in _SlimeStock) {
		//	var panel = _slimeStockCanvas.gameObject.transform.Find("Panel");
		//	var image = panel.Find(slime.Key.name.ToString());
		//	var text = image.Find("Num").GetComponent<Text>();
		//	text.text = "× " + slime.Value.ToString();
		//}

		// スライムの選択を行う
		SelectSlime();

		_OldSelection = _CurrentSelection;
	}

	protected virtual void LateUpdate() {
		if (_Animator.speed != _Player_Stats.SpeedMultiplyerProperties)
			_Animator.speed = _Player_Stats.SpeedMultiplyerProperties;

		if (Input.anyKeyDown) {
			int temp = 0;
			bool is_pressed = false;
			foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKey(vKey)) {
					if ((int) vKey >= (int) KeyCode.Alpha1 && (int) vKey <= (int) KeyCode.Alpha9) {
						temp = (int) vKey - (int) KeyCode.Alpha1;
						is_pressed = true;
						break;
					}
				}
			}

			if (is_pressed) {
				if (_ElementType.GetList().Count > temp) {
					StockSlime((ElementType) (_ElementType.GetList() [temp]));
				}
			}
		}

	}

	// スライムを選んでオーブリストに送る
	void SelectSlime() {
		Dictionary<KeyCode, ElementType> keyBind = new Dictionary<KeyCode, ElementType>();
		keyBind.Add(KeyCode.Z, _ElementType.GetList() [0] as ElementType);
		keyBind.Add(KeyCode.X, _ElementType.GetList() [1] as ElementType);
		keyBind.Add(KeyCode.C, _ElementType.GetList() [2] as ElementType);

		ElementType type = null;
		foreach (var key in keyBind) {
			if (Input.GetKeyDown(key.Key)) {
				// 溢れた分を仮ストックから抜く
				if (_tmpStock.Count >= _Orb_Slots) {
					ResetOrbSlots();
					_tmpStock.Clear();
					return;
				}

				type = key.Value;
			}
		}

		if (type == null)
			return;

		// 選んだ型のスライムの残数
		int slimeRemain = 0;
		_SlimeStock.TryGetValue(type, out slimeRemain);

		// 選んだ方のオーブストックに送った分
		int slot = CountSearchQueue(type, _tmpStock);

		// スライムがストックに1体以上残っていれば仮ストックに突っ込める
		if ((slimeRemain - slot) >= 1) {
			_tmpStock.Enqueue(type);
			StoreElementInOrb(type);

            AudioManager.Instance.PlaySE("OrbSet");
        }

    }

	int CountSearchQueue<T>(T target, Queue<T> list) {
		int result = 0;
		foreach (var content in list) {
			if (list.Contains(target)) {
				result++;
			}
		}
		return result;
	}

	// 一旦リストに格納する
	public void StockSlime(ElementType type) {
		AudioManager.Instance.PlaySE(_SuckSFX.name);

		switch (type.name) {
			case "Gold":
				_Money += type.GetRandomAmount();
				break;

			case "Heal":
				_Player_Stats.HealthProperties += type.GetRandomAmount();
				_Player_Stats.HealthProperties = Mathf.Clamp(_Player_Stats.HealthProperties, 0, MaxHitPoint);
				break;

			default:
				// 新生成用のストックに加算
				int slimeCount = 0;
				_SlimeStock.TryGetValue(type, out slimeCount);
				_SlimeStock.AddOrReplace(type, slimeCount + 1);
				break;
		}

	}

	// オーブを送ってスキルを生成する
	public void StoreElementInOrb(ElementType type) {
		string debug = "";

		if (_CurrentSkillOutcome != null) {
			Destroy(_CurrentSkillOutcome);
			_CurrentSkillOutcome = null;
		}

		_OrbSlot.Add(type);
		if (_OrbSlot.Count > _Orb_Slots)
			_OrbSlot.RemoveAt(0);

		bool HasUniqueCombination = true;

		if (_OrbSlot.Count >= _Orb_Slots) {
			for (int i = 0; i < _OrbSlot.Count; ++i) {
				for (int j = i + 1; j < _OrbSlot.Count; ++j) {
					if (_OrbSlot[i].Equals(_OrbSlot[j])) {
						HasUniqueCombination = false;
						break;
					}
				}
			}
		} else
			HasUniqueCombination = false;

		if (HasUniqueCombination) {
			foreach (Skill s in _combiSkill.GetList()) {
				var conbinationElements = s.GetCombinationElements();
				if (conbinationElements.Length > _OrbSlot.Count)
					continue;

				bool found_skill = true;

				for (int i = 0; i < _OrbSlot.Count; ++i) {
					if (conbinationElements[i] != _OrbSlot[i]) {
						found_skill = false;
						break;
					}
				}

				if (found_skill) {
					_CurrentSkillOutcome = ScriptableObject.Instantiate(s);
					_CurrentSkillOutcome.name = s.name;
					debug += "------------------------------------------------\n";
					debug += "[Created Skill] " + _CurrentSkillOutcome.name + "\n";
					debug += "------------------------------------------------";

					break;
				} else
					_CurrentSkillOutcome = null;
			}
		} else {
			foreach (Skill s in _baseSkill.GetList()) {
				if (_OrbSlot.Count <= 0)
					break;

				var baseElement = s.GetBaseElement();
				if (baseElement.Equals(_OrbSlot[0])) {
					int temp_tier = _OrbSlot.Skip(1).Where(x => x.Equals(baseElement)).Count();

					_CurrentSkillOutcome = ScriptableObject.Instantiate(s);
					_CurrentSkillOutcome.name = s.name;

					debug += "------------------------------------------------\n";
					debug += "[Created Skill] " + _CurrentSkillOutcome.name + "\n";

					// for (int i = 1; i < _OrbSlot.Count; ++i)
					// {
					//     if (baseElement.Equals(_OrbSlot[i]))
					//         ++temp_tier;
					//     else
					//         break;
					// }

					_CurrentSkillOutcome.SetSkillTier((SkillTier) _skillTier.GetList() [temp_tier]);
					debug += "[Setting Tier] " + _CurrentSkillOutcome.GetSkillTier() + "\n";
					// List<ElementType> temp = new List<ElementType>();
					// temp.AddRange(_OrbSlot);

					// temp.RemoveRange(0, temp_tier + 1);
					List<ElementType> temp = _OrbSlot.Skip(temp_tier + 1).ToList();

					if (temp.Count > 0) {
						_CurrentSkillOutcome.SetElementType(temp);

						var statusEffects = _CurrentSkillOutcome.GetStatusEffects();
						foreach (StatusEffect et in statusEffects)
							debug += "[Skill (" + _CurrentSkillOutcome.name + ") Status Added] " + et.name + "\n";
					}
					debug += "------------------------------------------------";
					break;
				} else
					_CurrentSkillOutcome = null;
			}
		}

		Debug.Log(debug);
	}

	protected void StoreSkills() {
		if (_CurrentSkillOutcome != null) {

			// 空いているスロットにスキルを登録する。なければ選択中のスロットに登録する。
			int storeSlotNumber = _CurrentSelection;
			var emptySlot = _Skills.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value == null);
			if (emptySlot != null) {
				storeSlotNumber = emptySlot.Index;
			}
			_Skills[storeSlotNumber] = _CurrentSkillOutcome;

			ResetOrbSlots();
			AudioManager.Instance.PlaySE(_SkillStoreSFX.name);

			_CurrentSkillOutcome = null;
		}
	}

	protected void UseSkill() {
		if (_Skills[_CurrentSelection] == null) {
			return;
		}
		_CurrentUseSkill = _Skills[_CurrentSelection];
		_CurrentUseSkill.Init();
		_Skills[_CurrentSelection] = null;
		_Is_Casting = true;
	}

	private void ResetCurrentUsedSkill() {
		_CurrentUseSkill.Reset();
		Destroy(_CurrentUseSkill);
		_Is_Casting = false;
	}

	public void ResetOrbSlots() {
		_OrbSlot.Clear();
		_CurrentSkillOutcome = null;
	}

	public EnumHolder.States GetPlayerState() {
		return _Player_State;
	}

	public Stats GetPlayerStats() {
		return _Player_Stats;
	}

	public bool IsDead() {
		return _Player_Stats.HealthProperties <= 0;
	}

	public bool IsCasting() {
		return _Is_Casting;
	}

	/*
	#if UNITY_EDITOR
		void OnGUI() {
			GUI.Box(new Rect(10, 10, 100, 50), "Orb Slots");

			for (int i = 0; i < _OrbSlot.Count; ++i) {
				GUI.Box(new Rect(10, 50 * (i + 1), 100, 50), _OrbSlot[i].name);
			}

			GUI.Box(new Rect(10, 50 * 5, 500, 50), "Output: " + ((_CurrentSkillOutcome != null) ? ((_CurrentSkillOutcome.GetSkillTier() != null) ? _CurrentSkillOutcome.GetSkillTier().name + " " : "") + _CurrentSkillOutcome.name : "None"));

			GUI.Box(new Rect(1500, 10, 100, 50), "Skill Slots");

			for (int i = 0; i < _Skills.Count; ++i) {
				if (_Skills[i] != null) {
					GUI.Box(new Rect(1500, 50 * (i + 1), 500, 50), ((_Skills[i].GetSkillTier() != null) ? _Skills[i].GetSkillTier().name + " " : "") + _Skills[i].name);
				}
			}

			if (_Skills.Count > 0) {
				if (_Skills[_CurrentSelection] != null) {
					GUI.Box(new Rect(1500, 50 * 5, 500, 50), ((_Skills[_CurrentSelection].GetSkillTier() != null) ? _Skills[_CurrentSelection].GetSkillTier().name + " " : "") + _Skills[_CurrentSelection].name);
					GUI.Box(new Rect(1500, 50 * 6, 500, 50), "Description: " + _Skills[_CurrentSelection].GetDescription());
				}
			}

			GUI.Box(new Rect(900, 10, 100, 50), "State: " + _Player_State);
		}
	#endif
	*/

	public void TakeDamage(float Damage) {
		if (Damage > 0) {
			_Animator.SetTrigger("IsDamage");
			_Player_Stats.HealthProperties -= Damage;
		}
	}

	public void StartSuck() {
		if (!_SuckingParticle.activeSelf)
			_SuckingParticle.SetActive(true);

		if (!_SuckingRadius.activeSelf)
			_SuckingRadius.SetActive(true);

		_SuckingParticle.transform.Rotate(Vector3.up, 15);
	}

	public List<ElementType> GetOrbsInSlot() {
		return _OrbSlot;
	}

	public List<Skill> GetSkillsInSlot() {
		return _Skills;
	}

	public int CurrentSelectedSkill() {
		return _CurrentSelection;
	}

	public Skill CurrentSkillOutcome() {
		return _CurrentSkillOutcome;
	}

	public Status GetStatus() {
		return _Status;
	}

	public void CallWalkParticle() {
		AudioManager.Instance.PlaySE(_WalkingSFX.name);
		GameObject temp = Instantiate(_WalkParticle, transform.position, transform.rotation);
		Destroy(temp, 0.5f);
	}

	delegate void CheckFunctions();
}