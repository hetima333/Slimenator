using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

public GameObject _bossPrefab;

	// Use this for initialization
	void Start () {
	if(_bossPrefab == null)
	{
		print("Bossプレハブがセットされていません");
		return;
	}

		ObjectManager.Instance.InstantiateWithObjectPooling(_bossPrefab, gameObject.transform.position, new Quaternion());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
