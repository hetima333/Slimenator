using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurringTest : MonoBehaviour {


	private Component[] _components;

	// Use this for initialization
	void Start () {
		_components = GetComponents<Component>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnWillRenderObject()
	{

	#if UNITY_EDITOR

		if(Camera.current.name != "SceneCamera"  && Camera.current.name != "Preview Camera")

	#endif
		{
			Debug.Log("写ってるはず");
				}	
		else
		{
		
		}
	}	

	// void AllComponentsEnable()
	// {
	// 	foreach (var cmp in _components)
	// 	{
	// 		cmp.;
	// 	}
	// }


	// void AllComponentsDisable()
	// {
	// 	foreach (var cmp in _components)
	// 	{
	// 		cmp.enabled = false;
	// 	}
	// }
}
