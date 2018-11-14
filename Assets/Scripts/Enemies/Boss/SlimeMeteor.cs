using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMeteor : MonoBehaviour {

	[SerializeField]
	GameObject _blast;

	Rigidbody _rid;

	// Use this for initialization
	void Start () {

		_rid = GetComponent<Rigidbody> ();
		_rid.velocity = new Vector3 (0, -50, 0);
	}

	// Update is called once per frame
	void Update () { }

	void OnCollisionEnter (Collision col) {
		Vector3 hitPos;

		foreach (ContactPoint point in col.contacts) {
			hitPos = point.point;
			Instantiate (_blast, hitPos, Quaternion.identity);
		}

		Destroy (gameObject);

	}
}