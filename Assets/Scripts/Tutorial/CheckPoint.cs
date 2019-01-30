using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

void OnTriggerEnter(Collider col)
{

	if(col.gameObject.tag == "Player")
	{
		GetComponentInParent<MoveChecker>().MoveCheck();
		gameObject.SetActive(false);
	}
}

}
