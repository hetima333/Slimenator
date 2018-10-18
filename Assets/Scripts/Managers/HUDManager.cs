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

	#region OrbSlot

	// スロットへの参照
	private OrbSlot[] _orbSlots = new OrbSlot[3];

	// 仮のコード
	// TODO : プレイヤーから情報を得る
	[SerializeField]
	private Orbs[] _orbs = new Orbs[3];

	#endregion

	#region Money

	// 表示される金額のテキスト
	[SerializeField]
	private Text _moneyText;

	#endregion

	// Use this for initialization
	void Start () {
		_playerStats = GameObject.Find("Player").GetComponent<IPlayerStats>();

		_playerHPBar.minValue = 0;
		_redHP = _playerStats.HitPoint;
		_redHPBar = _playerHPBar.GetComponentInChildren<Image>().rectTransform;
		_maxWidth = _redHPBar.sizeDelta.x;

		_orbSlots = GameObject.Find("OrbSlot").GetComponentsInChildren<OrbSlot>();
	}
	
	// Update is called once per frame
	void Update () {
		// HPBarの更新
		UpdateHPBar();

		// オーブスロットの更新
		UpdateOrbSlot();

		// 所持金の更新
		UpdateMoney();
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

	private void UpdateOrbSlot(){
		// オーブのデータを更新する
		for (int i = 0; i < _orbSlots.Length; i++){
			_orbSlots[i].Orb = _orbs[i];
		}
	}

	private void UpdateMoney(){
		// TODO : プレイヤーから取得する
		// 仮の数値
		var money = 3000;

		// 現在の所持金が変動していなかったら更新を行わない
		int result;
		if(int.TryParse(_moneyText.text, out result)){
			if(result == money){
				return;
			}
		}

		// 表示テキストの更新(3桁ごとにカンマ)
		_moneyText.text = money.ToString("N0");
	}
}
