using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private Mask _playerHitPoints;

	private List<Image> _playerHearts = new List<Image>();

	private IPlayerStats _playerStats;

	// Use this for initialization
	void Start () {
		_playerStats = GameObject.Find("Player").GetComponent<IPlayerStats>();

		// ハートの参照をとる
		foreach(var item in _playerHitPoints.GetComponentsInChildren<Image>().Skip(1)){
			_playerHearts.Add(item);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// _text.text = _playerStats.HitPoint + " / " + _playerStats.MaxHitPoint;

		for (int i = 0; i < _playerStats.MaxHitPoint; i++){
			bool f = i < _playerStats.HitPoint;
			_playerHearts[i].color = f ? Color.red : Color.white;
		}
	}
}
