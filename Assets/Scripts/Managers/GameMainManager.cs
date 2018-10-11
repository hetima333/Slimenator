using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour {

	// TODO : PlayerDummyはPlayerに変更する
	[SerializeField]
	private PlayerDummy _player;

	public PlayerDummy Player{
		get { return _player; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
