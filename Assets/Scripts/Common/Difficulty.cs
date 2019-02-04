using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour {

public readonly static Difficulty Instance = new Difficulty();
	//難易度
	public string _difficulty = "NORMAL"; 
	//ステータス補正値
	public float _statusMagnification = 1.0f;
}
