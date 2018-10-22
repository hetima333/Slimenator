using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUDManager : MonoBehaviour {

	private IPlayerStats _playerStats;

	#region HPBar

	private HPBarCore _hpbar;

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

		_orbSlots = GameObject.Find("OrbSlot").GetComponentsInChildren<OrbSlot>();

		_hpbar = GameObject.Find("HPBar").GetComponent<HPBarCore>();
	}
	
	// Update is called once per frame
	void Update () {
		// オーブスロットの更新
		UpdateOrbSlot();

		// 所持金の更新
		UpdateMoney();
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
