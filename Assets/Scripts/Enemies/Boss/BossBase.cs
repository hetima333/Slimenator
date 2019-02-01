using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SimpleAnimation))]
public abstract class BossBase : MonoBehaviour, IDamageable,ISuckable {

	public enum State {
		ALIVE,
		DEAD
	}

	public State _state = State.ALIVE;

	protected const float ACT_INTERVAL = 1.0f;
	protected float _actInterval = ACT_INTERVAL;

	[SerializeField]
	public bool _isGround = true;

	public bool IsGround { get { return _isGround; } }

	public GameObject _target;

	public bool _isAction = false;

	protected int _phase = 0;

	//アニメーション
	public SimpleAnimation _anim;

	public string _animName;

	public float _animLastTime = 0;

	//Object for long range attack
	public GameObject _slimeBullet;

	public BossSkill _previousSkill;


	public bool _isSacking = false;

	[SerializeField]
	//発動できるスキルリスト
	public List<BossSkill> _skillList = new List<BossSkill> ();

	public List<BossSkill> _canUseSkillList = new List<BossSkill> ();

	[SerializeField]
	public bool _canAnimation = true;

	public GameObject _shockWave;

	protected Status _status;
	public Stats _properties;

	//最大値
	public float MaxHitPoint { get { return _properties.MaxHealthProperties * _properties.HealthMultiplyerProperties; } }
	//体力
	public float HitPoint { get { return _properties.HealthProperties; } }

	public abstract void Init (Stats stat);


	public bool _isLady = false;

	public void Awake () {
		_shockWave = Resources.Load ("EnemyItem/ShockWave", typeof (GameObject)) as GameObject;
	}


	void OnDisable()
    {
		//再生中のアニメーションの再生位置を記憶
		//Debug.Log(_anim.GetState(_animName).time);
		_animLastTime = _anim.GetState(_animName).time-(int)_anim.GetState(_animName).time;
    }

    void OnEnable()
    {
		if(!_isLady)return;
		//再生中のアニメーションがあれば
		if(_animName != null)
		{	
			//最後のアニメーションの再生位置を記憶しておいたものに変更
			_anim.GetState (_animName).normalizedTime = _animLastTime;
			//最後のアニメーション再生
			_anim.CrossFade(_animName,0);
		}
    }




	public void TakeDamage (float damage) {
		if (_state == State.DEAD) return;
		if(damage > 5000.0f){damage =1;_isSacking=true;}
		_properties.HealthProperties -= damage;
	}

	private void OnCollisionEnter (Collision col) {

		if (col.gameObject.layer == LayerMask.NameToLayer ("Player") && _state == BossBase.State.ALIVE) {
			//Make sure the target has components
			var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

			//If have a component
			if (hasIDamageableObject != null &&_isAction == true) {
				//ダメージ判定
				//TODO take damage   
				hasIDamageableObject.TakeDamage (10);
				_isAction = false;
			}
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && _state == BossBase.State.ALIVE) {
			if (_isGround == false&&_isAction == true) {
				if (_canAnimation) {
					_anim.CrossFade ("Fall", 0);
					_animName = "Fall";
					if (_shockWave) {
						GameObject shockWave = Instantiate (_shockWave);
						shockWave.GetComponent<ShockWave> ().SetScale (35);
						shockWave.GetComponent<ShockWave> ().SetDamage (10);
						//接触地点を取得
						Vector3 ShockPos = gameObject.transform.position;
						ShockPos.y = 1f;
						shockWave.transform.position = ShockPos;
						}
					}
				_isGround = true;
			}
		}
	}

	//★アニメーションを状態異常に適応させるためのLateUpdate
	protected virtual void LateUpdate () {
		//活動開始まではアニメーション速度に変動を与えない
		if(_phase == 0)return;
		//現在再生中のアニメがある
		if (_anim.GetState (_animName) != null)
			//再生中のアニメのスピードが想定される再生速度と異なる場合
			if (_anim.GetState (_animName).speed != _properties.SpeedMultiplyerProperties) {
				//再生速度の変更を行う
				_anim.GetState (_animName).speed = _properties.SpeedMultiplyerProperties;
			}
	}

	private void OnCollisionExit (Collision col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && _state == BossBase.State.ALIVE) {
			_isGround = false;
		}
	}

	public void SetStatus () {
		_properties.HealthProperties = MaxHitPoint;
		_status = gameObject.GetComponent<Status> ();
		_status.Init ();
	}


	//★吸い込まれた時呼ばれる関数
    public void Sacking () {
        return;
    }

}