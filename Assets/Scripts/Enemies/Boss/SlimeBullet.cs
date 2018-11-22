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

	[SerializeField]
	public SOList
	_elements;

	[SerializeField]
	public SkillTier
	_startingTier;

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
			int random = Random.Range (0, _elements.GetList ().Count);
			GetSlimeFromPool (random, gameObject.transform.position);
		}

		//Release bullet
		ObjectManager.Instance.ReleaseObject (gameObject);
	}

	public GameObject GetSlimeFromPool (int type, Vector3 position = new Vector3 ()) {
		GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling (_prefab, position);
		Stats temp = EnumHolder.Instance.GetStats (_prefab.name);
		SlimeBase temp_component = slime_obj.GetComponent<SlimeBase> ();

		if (temp_component != null)
			DestroyImmediate (temp_component);

		System.Type _MyScriptType = System.Type.GetType (((ElementType) _elements.GetList () [type]).GetSlimeScriptName ());
		slime_obj.AddComponent (_MyScriptType);

		slime_obj.GetComponent<SlimeBase> ().Init (temp, ((((ElementType) _elements.GetList () [type]).name.Equals ("Lightning")) ? 2 : 1), ((ElementType) _elements.GetList () [type]), _startingTier);
		slime_obj.SetActive (true);

		return slime_obj;
	}
}