using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCore : MonoBehaviour {

	[SerializeField]
	private HUDManager _manager;

	public float MoneyAmount {
		get {
			return _manager.Player.MoneyAmount;
		}
	}
}