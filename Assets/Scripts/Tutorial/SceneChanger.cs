using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {


[SerializeField]
private string _sceneName;
	void OnTriggerEnter(Collider col)
{

	if(col.gameObject.tag == "Player")
	{
		FadeManager.Instance.StartTransition(1.0f,_sceneName);
	}
}

}
