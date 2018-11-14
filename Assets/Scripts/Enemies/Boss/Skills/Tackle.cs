using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : BossSkill {

	// Use this for initialization

	private Rigidbody _rid;
	void Start () {
		_rid = gameObject.GetComponent<Rigidbody> ();
		_maxCoolTime = 7;
		_coolTime = _maxCoolTime;
		_target = GameObject.Find ("Player");
	}

	override public void Action () {
		_actTime = 2.0f;
		Vector3 lookPos = _target.transform.position;
		lookPos.y = gameObject.transform.position.y;
		transform.LookAt (lookPos);

		//相手に向かって体当たりを開始。
		_rid.AddForce (transform.forward * 50000);

		Debug.Log ("ぶちかます");

	}

}