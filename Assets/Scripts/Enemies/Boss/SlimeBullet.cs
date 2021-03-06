﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour {

	public float _speed;
	private Vector3 forward;
	private Quaternion forwardAxis;
	private Rigidbody rb;
	private GameObject characterObject;

	private int _randomNumber =0;

	[SerializeField]
	private GameObject _prefab;

	[SerializeField]
	private Material[] _mat;

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
		//生成するスライムの番号をランダムで決める
		_randomNumber =  Random.Range (0, _elements.GetList ().Count - 1);
		//色変え
		GetComponent<Renderer>().material = _mat[_randomNumber]; 
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
			//ステージに存在するスライムの数が指定数以下なら
			if(ObjectManager.Instance.GetActiveObjects(_prefab).Count < 20){
				//ランダムなスライムを生成する
			
				GetSlimeFromPool (_randomNumber, gameObject.transform.position);
			}
			
		}
		
		//Release bullet
		ObjectManager.Instance.ReleaseObject (gameObject);
	}

	public GameObject GetSlimeFromPool (int type, Vector3 position = new Vector3 ()) {
		GameObject slime_obj = ObjectManager.Instance.InstantiateWithObjectPooling (_prefab, position);
		Stats temp = EnumHolder.Instance.GetStats (_prefab.name);
		SlimeBase temp_component = slime_obj.GetComponent<SlimeBase> ();

		if (temp_component != null)
			Destroy (temp_component);

		System.Type _MyScriptType = System.Type.GetType (((ElementType) _elements.GetList () [type]).GetSlimeScriptName ());
		SlimeBase temp_script = slime_obj.AddComponent (_MyScriptType) as SlimeBase;

		temp_script.Init (temp, ((((ElementType) _elements.GetList () [type]).name.Equals ("Lightning")) ? 2 : 1), ((ElementType) _elements.GetList () [type]), _startingTier);
		slime_obj.SetActive (true);

		return slime_obj;
	}
}