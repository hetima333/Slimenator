using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SimpleAnimation))]
public abstract class BossBase : MonoBehaviour, IDamageable {

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

	//Object for long range attack
	public GameObject _slimeBullet;

	public BossSkill _previousSkill;

	[SerializeField]
	//発動できるスキルリスト
	public List<BossSkill> _skillList = new List<BossSkill> ();

	public List<BossSkill> _canUseSkillList = new List<BossSkill> ();

	[SerializeField]
	public bool _canAnimation = true;

	public GameObject _shockWave;

	protected Status _status;
	protected Stats _properties;

	//最大値
	public float MaxHitPoint { get { return _properties.MaxHealthProperties * _properties.HealthMultiplyerProperties; } }
	//体力
	public float HitPoint { get { return _properties.HealthProperties; } }

	public abstract void Init (Stats stat);

	public void Awake () {
		_shockWave = Resources.Load ("EnemyItem/ShockWave", typeof (GameObject)) as GameObject;
	}

	public void TakeDamage (float damage) {
		_properties.HealthProperties -= damage;
	}

	private void OnCollisionEnter (Collision col) {

		if (col.gameObject.layer == LayerMask.NameToLayer ("Player") && _state == BossBase.State.ALIVE) {
			//Make sure the target has components
			var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

			//If have a component
			if (hasIDamageableObject != null) {
				//ダメージ判定
				//TODO take damage   
				hasIDamageableObject.TakeDamage (10);
			}
		}

		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && _state == BossBase.State.ALIVE) {
			if (_isGround == false) {
				Debug.Log ("着地");
				if (_isAction)
					if (_canAnimation) {
						_anim.CrossFade ("Fall", 0);
					}

				if (_shockWave) {
					GameObject shockWave = Instantiate (_shockWave);
					shockWave.GetComponent<ShockWave> ().SetScale (35);
					shockWave.GetComponent<ShockWave> ().SetDamage (10);
					//接触地点を取得
					Vector3 ShockPos = gameObject.transform.position;
					ShockPos.y = 1f;
					shockWave.transform.position = ShockPos;
				}
				_isGround = true;
			}
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

}