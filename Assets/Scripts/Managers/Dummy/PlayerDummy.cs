using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour, IPlayerStats {

	[SerializeField, Range(0, 100)]
	private int _hitPoint;
	public int HitPoint{
		get { return _hitPoint; }
	}

	private const int MAX_HEALTH = 100;
	public int MaxHitPoint{
		get { return MAX_HEALTH; }
	}

	// Use this for initialization
	void Start () {
		_hitPoint = MAX_HEALTH;
	}
	
	// Update is called once per frame
	void Update () {
		// HP減少テスト
		if(Input.GetKeyDown(KeyCode.O)){
			_hitPoint--;
		}
	}
}
