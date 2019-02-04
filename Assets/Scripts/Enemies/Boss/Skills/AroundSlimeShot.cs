using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundSlimeShot : BossSkill {

	private GameObject _slimeBullet;

	// Use this for initialization
	void Start () {
		_slimeBullet = Resources.Load ("EnemyItem/SlimeBullet", typeof (GameObject)) as GameObject;
		_Type = AttackType.SHOT;
	}

	override public void Action () {
		_rid.velocity = Vector3.zero;
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Shot2", 0);
			_boss._animName = "Shot2";
		} else {
			for (int i = 0; i < 11; i++) {

				_canActive = false;
				CreateShotObject (i * 30.0f);

			}
			_actTime = 1.0f;
			_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;
		}

	}

	private void CreateShotObject (float axis) {
		GameObject shot = Instantiate (_slimeBullet, transform.position, Quaternion.identity);
		var shotObject = shot.GetComponent<SlimeBullet> ();
		shotObject.SetCharacterObject (gameObject);
		shotObject.SetForwordAxis (Quaternion.AngleAxis (axis, Vector3.up));
	}

	void ShotStart () {
		for (int i = 0; i < 12; i++) {	
			CreateShotObject (i * 30.0f);
		}
		_actTime = 1.0f;
		AudioManager.Instance.PlaySE("SlimeShot");
		_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;
	}

		void ShotEnd () {
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Idle", 0);
			_boss._animName = "Idle";
		}
	}
}