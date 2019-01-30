using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChecker : MonoBehaviour {

[SerializeField]
private GameObject[] _points;


	public int _moveStep = 1;

	// Use this for initialization
	void Start () {
		_moveStep = 1;
	}
	


	public void MoveCheck()
	{
		
		if(_moveStep <4)
		{
			_points[_moveStep].SetActive(true);
			_moveStep++;
		}
		else{
			TutorialManager.Instance.StepUp();
		}
	}


}
