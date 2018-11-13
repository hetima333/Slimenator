using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour {

	public float _speed;
	private Vector3 forward;
	private Quaternion forwardAxis;
	private Rigidbody rb;
	private GameObject characterObject;

	[SerializeField]
	private GameObject _prefab;

	private float _damage = 10;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		forward = characterObject.transform.forward;
	}

	void Update () {
		rb.velocity = forwardAxis * forward * _speed;
	}

	public void SetCharacterObject (GameObject characterObject) {
		this.characterObject = characterObject;
	}

	public void SetForwordAxis (Quaternion axis) {
		this.forwardAxis = axis;
	}

	//自分の本体に何かが接触した場合
	void OnCollisionEnter (Collision col) {

		//Make sure the target has components
		var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

		//If have a component
		if (hasIDamageableObject != null) {
			//ダメージ判定
			//TODO take damage   
			hasIDamageableObject.TakeDamage (_damage);
		} else {

		}

		//Release bullet
		ObjectManager.Instance.ReleaseObject (gameObject);
	}
}