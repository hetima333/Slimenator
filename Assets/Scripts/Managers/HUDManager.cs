using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUDManager : MonoBehaviour {

	private IPlayerStats _playerStats;

	#region HPBar

	[SerializeField]
	private Slider _playerHPBar;
	// 減少中のHP
	private float _redHP;

	// HP減少の速度
	[SerializeField, Range(0.01f, 2.0f)]
	private float _hpDecreaseSpeed;

	private RectTransform _redHPBar;
	private float _maxWidth;
		
	#endregion

	

	// Use this for initialization
	void Start () {
		_playerStats = GameObject.Find("Player").GetComponent<IPlayerStats>();

		_playerHPBar.minValue = 0;
		_redHP = _playerStats.HitPoint;
		_redHPBar = _playerHPBar.GetComponentInChildren<Image>().rectTransform;
		_maxWidth = _redHPBar.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update () {
		// HPBarの更新
		UpdateHPBar();
	}

	private void UpdateHPBar(){
		// 最大HPと現在HPの更新
		_playerHPBar.maxValue = _playerStats.MaxHitPoint;
		_playerHPBar.value = _playerStats.HitPoint;

		// 緑HPより赤HPが多ければ赤HPをへらす
		if(_redHP > _playerHPBar.value){
			_redHP -= _hpDecreaseSpeed;
		}
		else{
			_redHP = _playerHPBar.value;
		}

		// HPバーにサイズを適用する
		_redHPBar.sizeDelta = new Vector2(_redHP * _maxWidth / _playerHPBar.maxValue, _redHPBar.sizeDelta.y);

	}
}
