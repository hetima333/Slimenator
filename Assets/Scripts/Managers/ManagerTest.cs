using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Start : " + ManagerCore.Instance.Hud.name);
		Debug.Log("Start : " + ManagerCore.Instance.Audio.name);
		Debug.Log("Start : " + ManagerCore.Instance.Objects.name);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
