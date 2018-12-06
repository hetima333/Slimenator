using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMeteorRain : BossSkill {

	private GameObject _slimeMeteor;

	private GameObject _meteor;

	// Use this for initialization
	void Start () {
		_slimeMeteor = Resources.Load ("EnemyItem/SlimeMeteor", typeof (GameObject)) as GameObject;
	}

	override public void Action () {
		Debug.Log ("メテオ");
		_actTime = 1.0f;

		MeteorFall (30);
		_canActive = false;

		_boss._isAction = true;

	}

	private void MeteorFall (int num) {

		for (int i = 0; i < num; i++) {
			Vector3 pos = gameObject.transform.position;

			pos.x += Random.Range (-50, 50);
			pos.y = 50;
			pos.z += Random.Range (-50, 50);
			_meteor = Instantiate (_slimeMeteor, pos, Quaternion.identity);
		}

	}
}