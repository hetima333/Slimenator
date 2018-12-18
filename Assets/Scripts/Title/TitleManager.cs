using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioManager.Instance.StopBGM();
		AudioManager.Instance.PlayBGM("TitleBGM",0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
