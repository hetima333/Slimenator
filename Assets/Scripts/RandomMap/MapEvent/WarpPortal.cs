using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour {

[SerializeField]
WarpPortal _transferTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(_transferTarget == null)
			return;

		if(other.gameObject.tag == "Player")
		{
			print("warp!");
			other.gameObject.transform.position = _transferTarget.transform.position;
		}
	}
}
