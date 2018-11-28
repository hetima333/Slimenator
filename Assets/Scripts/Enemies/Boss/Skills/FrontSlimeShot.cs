using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontSlimeShot : BossSkill {

	private GameObject _slimeBullet;

	// Use this for initialization
	void Start () {
		_slimeBullet = Resources.Load ("EnemyItem/SlimeBullet", typeof (GameObject)) as GameObject;
		_Type = AttackType.SHOT;
	}

	override public void Action () {
		_rid.velocity = Vector3.zero;
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Shot", 0);
			_boss._animName = "Shot";
		} else {
			Debug.Log ("射撃");
			_actTime = 1.0f;
			CreateShotObject (0);
			CreateShotObject (30f);
			CreateShotObject (-30f);
		}
		_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;

	}

	private void CreateShotObject (float axis) {
		GameObject shot = Instantiate (_slimeBullet, transform.position, Quaternion.identity);
		var shotObject = shot.GetComponent<SlimeBullet> ();
		shotObject.SetCharacterObject (gameObject);
		shotObject.SetForwordAxis (Quaternion.AngleAxis (axis, Vector3.up));
	}

	void ShotStart () {
		_actTime = 1.0f;
		CreateShotObject (0);
		CreateShotObject (30f);
		CreateShotObject (-30f);
	}

	void ShotEnd () {
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Idle", 0);
			_boss._animName = "Idle";
		}
	}
}