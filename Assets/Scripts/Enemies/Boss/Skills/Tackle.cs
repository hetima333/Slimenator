using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : BossSkill {

	// Use this for initialization
	void Start () {

	}

	override public void Action () {

		Vector3 lookPos = _target.transform.position;

		lookPos.y = gameObject.transform.position.y;

		transform.LookAt (lookPos);

		Debug.Log ("ぶちかます");

	}

}