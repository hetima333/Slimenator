using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : BossSkill {

	// Use this for initialization
	void Start () {
		_rid = gameObject.GetComponent<Rigidbody> ();
		_maxCoolTime = 7;
		_coolTime = _maxCoolTime;
		_target = GameObject.Find ("Player");
		_Type = AttackType.PHYSICAL;
	}

	override public void Action () {
		_actTime = 2.0f;
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Tackle", 0);
			_boss._animName = "Tackle";
		}

		Vector3 lookPos = _target.transform.position;
		lookPos.y = gameObject.transform.position.y;
		transform.LookAt (lookPos);

		//相手に向かって体当たりを開始。
		_rid.AddForce (transform.forward * 5000*_rid.mass);
		//速度調整
		_rid.velocity = _rid.velocity * _boss._properties.SpeedMultiplyerProperties;
		_canActive = false;
		//行動中にする
		_boss.GetComponent<BossBase> ()._isAction = true;
	
	}

}