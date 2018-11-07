using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundSlimeShot : BossSkill {

	private GameObject _slimeBullet;

	// Use this for initialization
	void Start () {
		_slimeBullet = Resources.Load ("EnemyItem/SlimeBullet", typeof (GameObject)) as GameObject;
	}

	override public void Action () {
		for (int i = 0; i < 12; i++) {
			CreateShotObject (i * 30.0f);

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
}