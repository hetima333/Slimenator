using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMeteor : MonoBehaviour {

	[SerializeField]
	GameObject _blast;

	[SerializeField]
	GameObject _marker;

	GameObject marker;
	Rigidbody _rid;

	// Use this for initialization
	void Start () {

		_rid = GetComponent<Rigidbody> ();
		_rid.velocity = new Vector3 (0, -50, 0);

		Ray ray = new Ray (transform.position, new Vector3 (0, -1, 0));

		Debug.DrawRay (ray.origin, ray.direction * Mathf.Infinity, Color.red, 3, false);
		// 2.		
		// Rayが衝突したコライダーの情報を得る
		RaycastHit hit;
		// Rayが衝突したかどうか
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, LayerMask.GetMask ("Ground"))) {

			marker = Instantiate (_marker);

			//落下予測画像をレイが当たった位置に移動させる
			var pos = hit.point;
			pos.y = 0.1f;
			marker.transform.position = pos;
		}

	}

	// Update is called once per frame
	void Update () { }

	void OnCollisionEnter (Collision col) {
		Vector3 hitPos;

		foreach (ContactPoint point in col.contacts) {
			hitPos = point.point;
			Instantiate (_blast, hitPos, Quaternion.identity);
		}
		Destroy (marker);
		Destroy (gameObject);

	}
}