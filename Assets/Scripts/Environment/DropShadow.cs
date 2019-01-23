using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float parentHeight = gameObject.transform.parent.gameObject.transform.position.y;
		float rate = parentHeight*0.9f;
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, parentHeight, gameObject.transform.position.z);
	}
}
