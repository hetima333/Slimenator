using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgPopTest : MonoBehaviour {

	public GameObject _obj;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			DamagePoper.Instance.PopDamage(_obj.transform.position, Random.Range(1, 201));
			// Instantiate(_dmg.gameObject, _pos, Quaternion.identity).transform.SetParent(this.transform);
		}

	}
}