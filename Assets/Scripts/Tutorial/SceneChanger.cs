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
		SceneManager.LoadScene(_sceneName);
		if(_sceneName == "PlayTestScene")
		{
			Application.LoadLevelAdditive ("HUD");
		}
	}
}

}
