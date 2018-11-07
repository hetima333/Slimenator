using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontSlimeShot : BossSkill {

	private GameObject _slimeBullet;

	// Use this for initialization
	void Start () {
		_slimeBullet = Resources.Load ("EnemyItem/SlimeBullet", typeof (GameObject)) as GameObject;
	}

	override public void Action () {
		CreateShotObject (0);
		CreateShotObject (30f);
		CreateShotObject (-30f);
		_canActive = false;
		_boss.GetComponent<TestBoss> ()._isAction = true;

	}

	private void CreateShotObject (float axis) {
		GameObject shot = Instantiate (_slimeBullet, transform.position, Quaternion.identity);
		var shotObject = shot.GetComponent<SlimeBullet> ();
		shotObject.SetCharacterObject (gameObject);
		shotObject.SetForwordAxis (Quaternion.AngleAxis (axis, Vector3.up));
	}
}