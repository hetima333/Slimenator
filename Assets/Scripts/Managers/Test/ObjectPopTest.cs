using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPopTest : MonoBehaviour {

	[SerializeField]
	private GameObject _prefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// left click to spawn cube
		if(Input.GetMouseButtonDown(0)){
			ObjectManager.Instance.InstantiateWithObjectPooling(_prefab);
		}
	}

	void OnGUI(){
		GUI.Box(new Rect(10, 10, 100, 50), "Left Click");
	}
}
