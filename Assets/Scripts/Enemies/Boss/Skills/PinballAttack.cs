﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballAttack : BossSkill {

	[SerializeField]
	private GameObject _marker;

	private Rigidbody _rid;

	private GameObject _avatar;

	private void Start () {
		_rid = gameObject.GetComponent<Rigidbody> ();
		_target = GameObject.Find ("Player");
		_maxCoolTime = Random.Range (3, 6);
		_coolTime = _maxCoolTime;
		_avatar = gameObject.GetComponent<BossTwins> ()._avatar;
	}

	override public void Action () {
		if (!_avatar) return;
		_maxCoolTime = Random.Range (3, 6);
		_coolTime = _maxCoolTime;

		_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;

		Attack ();
		_avatar.GetComponent<PinballAttack> ().Attack ();

	}

	public void Attack () {
		//フィジックスマテリアルの有効

		Vector3 lookPos = _avatar.transform.position;
		lookPos.y = gameObject.transform.position.y;
		transform.LookAt (lookPos);

		//相手に向かって体当たりを開始。
		_rid.AddForce (transform.forward * 50000);

		Debug.Log ("体当たり");
	}
}