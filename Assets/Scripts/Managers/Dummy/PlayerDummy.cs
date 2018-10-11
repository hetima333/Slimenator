using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour {

	const int MAX_HEALTH = 3;
	public int Health{
		get; set;
	}

	// Use this for initialization
	void Start () {
		Health = MAX_HEALTH;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
