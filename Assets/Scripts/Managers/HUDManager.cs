﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private Text _text;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = "" + ManagerCore.Instance.Game.Player.Health;
	}
}
