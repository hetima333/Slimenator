using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour {

	//スキルの攻撃タイプ
	public enum AttackType {
		//物理的な攻撃
		PHYSICAL,
		//射撃
		SHOT
	}


	public AttackType _Type;

	public float _maxCoolTime = 5.0f;
	public float _coolTime;

	public float _actTime = 3.0f;
	public bool _canActive = true;
	public bool _isActive = false;

	public GameObject _target;

	public BossBase _boss;

	public Collider _col;

	public Rigidbody _rid;

	private void Awake () {
		_boss = this.gameObject.GetComponent<BossBase> ();

		_coolTime = _maxCoolTime;

		_target = _boss.GetComponent<BossBase> ()._target;

		_col = gameObject.GetComponent<Collider> ();

		_rid = gameObject.GetComponent<Rigidbody> ();

	}

	void Update () {

		if (_boss._state == BossBase.State.DEAD) return;

		if (!_isActive && !_canActive) {
			//クールタイム減算（状態異常による速度低下も考慮）
			_coolTime -= Time.deltaTime*_boss._properties.SpeedMultiplyerProperties;
			if (_coolTime <= 0) {
				_coolTime = _maxCoolTime;
				_canActive = true;
			}

			if (_actTime > 0) {
				//行動時間減算（状態異常による速度低下も考慮）
				_actTime -= Time.deltaTime*_boss._properties.SpeedMultiplyerProperties;
				if (_actTime <= 0) {
					ActEnd ();
				}
			}

		}
	}

	public virtual void Action () { }

	public void ActEnd () {
		_actTime = 0;
		_rid.velocity = Vector3.zero;
		_col.material = null;
		_isActive = false;
		_boss.GetComponent<BossBase> ()._isAction = false;
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Idle", 0);
		}
	}
}