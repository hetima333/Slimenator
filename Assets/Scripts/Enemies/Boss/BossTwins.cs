using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwins : BossBase {

	public enum Type {

		PHYSICAL,
		SHOT
	}

	[SerializeField]
	private Type _type;

	//TODO Boss Performance
	const float MONEY = 2000.0f;
	public GameObject _avatar;

	public bool _isAlone = false;

	private float graceTime = 5f;

	public override void Init (Stats _stat) {
		_properties = _stat;

		SetStatus ();
		_anim = this.gameObject.GetComponent<SimpleAnimation> ();	
		//分裂アニメの再生開始位置を終点に設定
		_anim.GetState ("WakeUp").normalizedTime = 0.9f;
		//再生速度を逆に
		_anim.GetState ("WakeUp").speed = -1f;

		//アニメーションの再生
		_anim.CrossFade ("WakeUp", 0);
		_animName = "WakeUp";
		_target = GameObject.Find ("Player");
		_isLady = true;

	}

	// Update is called once per frame
	void Update () {

		//★ステータスの更新
		_status.UpdateStatMultiplyer (ref _properties);

		//★状態ダメージを受ける
		TakeDamage (_status.GetValue (EnumHolder.EffectType.TAKEDAMAGE));
		
		if (_state == State.DEAD) return;

		_actInterval -= Time.deltaTime; {
			if (_actInterval <= 0) {
				switch (_phase) {
					case 1:
						_actInterval = ACT_INTERVAL;
						break;

					case 2:
						_actInterval = ACT_INTERVAL / 2.0f;
						break;
					default:

						break;
				}

				_actInterval = ACT_INTERVAL;
				UseSkill ();
			}
		}

		//１人になった場合
		if (_isAlone) {
			graceTime -= Time.deltaTime;
			if (graceTime <= 0) {
				PhaseUp ();
				_isAlone = false;
			}
		}

	}

	//ダメージを受ける
	public new void TakeDamage (float damage) {

		if (_state == State.DEAD) return;

		_properties.HealthProperties -= damage;
		if (_properties.HealthProperties <= 0) {
			_anim.CrossFade ("Dead", 0);
			_animName = "Dead";
			_state = State.DEAD;
		}

	}

	private void UseSkill () {

		if (_isAction) { Debug.Log ("今忙しい"); return; }

		_canUseSkillList.Clear ();

		foreach (var skill in _skillList) {
			if (skill._canActive == true) {
				_canUseSkillList.Add (skill);
			}
		}

		if (_canUseSkillList.Count == 0) { Debug.Log ("今使えるの無い"); return; }

		var pinBallComponent = GetComponent<PinballAttack> ();

		//ピンボール攻撃が出来るかつ相手が設置状態
		if (pinBallComponent._canActive == true && _avatar.GetComponent<BossTwins> ().IsGround == true) {
			if (_isGround && _previousSkill != pinBallComponent) {
				pinBallComponent.Action ();
				_avatar.GetComponent<PinballAttack> ().Action ();
				_previousSkill = pinBallComponent;
				_avatar.GetComponent<BossTwins> ()._previousSkill = _avatar.GetComponent<PinballAttack> ();
				return;
			}
		}

		Vector3 lookPos = _target.transform.position;

		lookPos.y = gameObject.transform.position.y;

		transform.LookAt (lookPos);

		//ランダムなスキルを呼び出す。
		if (_canUseSkillList.Count != 0) {
			var skill = _canUseSkillList[Random.Range (0, _canUseSkillList.Count)];
			//一回前に使ったスキルではない。かつ発動できる場合実行
			if (skill != _previousSkill) {
				skill.Action ();
				_previousSkill = skill;
				_canUseSkillList.Remove (skill);

			} else {
				Debug.Log ("被っとるんじゃ");
			}
		}

	}

	private void PhaseUp () {
		_phase++;
		Debug.Log ("Phase" + _phase);

		switch (_phase) {
			case 1:
				//タイプ別にスキルの追加
				switch (_type) {
					case Type.PHYSICAL:
						_skillList.Add (gameObject.AddComponent<JumpPress> ());
						_skillList.Add (gameObject.AddComponent<Tackle> ());
						break;
					case Type.SHOT:
						_skillList.Add (gameObject.AddComponent<FrontSlimeShot> ());
						_skillList.Add (gameObject.AddComponent<AroundSlimeShot> ());
						break;
				}
				_skillList.Add (gameObject.AddComponent<PinballAttack> ());
				break;

			case 2:
				

				//タイプ別に(相方の)スキルの追加
				switch (_type) {
					case Type.PHYSICAL:
						_skillList.Add (gameObject.AddComponent<FrontSlimeShot> ());
						_skillList.Add (gameObject.AddComponent<AroundSlimeShot> ());
						break;
					case Type.SHOT:
						_skillList.Add (gameObject.AddComponent<JumpPress> ());
						_skillList.Add (gameObject.AddComponent<Tackle> ());
						break;
				}
				//特殊技メテオの追加
				_skillList.Add (gameObject.AddComponent<SlimeMeteorRain> ());
				break;
			default:

				break;
		}

	}

	public void SetAvatar (GameObject avatar) {
		_avatar = avatar;
	}

	public void DeadCall () {
		if (_avatar != null) {
			//相方に一人になったと自覚させる
			_avatar.GetComponent<BossTwins> ()._isAlone = true;
			//生き残った方のピンボール封印
			_avatar.GetComponent<BossBase>()._skillList.Remove (gameObject.GetComponent<PinballAttack> ());
		}
		ObjectManager.Instance.ReleaseObject (gameObject);
	}

	void WakeUp () {
		PhaseUp ();
	}

}