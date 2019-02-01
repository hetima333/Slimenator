using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

	public GameObject _bossPrefab;

	// Use this for initialization
	void Start () {
		if (_bossPrefab == null) {
			print ("Bossプレハブがセットされていません");
			return;
		}

		GameObject obj = ObjectManager.Instance.InstantiateWithObjectPooling(_bossPrefab, gameObject.transform.position, new Quaternion ());
		obj.transform.Rotate(new Vector3(0,180,0));
		BossBase boss = obj.GetComponent<BossBase> ();
		boss.Init (EnumHolder.Instance.GetStats (_bossPrefab.name));
	}

	// Update is called once per frame
	void Update () {

	}
}