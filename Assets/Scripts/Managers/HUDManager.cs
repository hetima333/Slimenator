using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	private IPlayerStats _playerStats;

	// プレイヤーへの参照
	private EntityPlayer _player;
	public EntityPlayer Player {
		get {
			if (_player == null) {
				_player = GameObject.Find("Player").GetComponent<EntityPlayer>();
			}
			return _player;
		}
	}

	// 表示される金額のテキスト
	[SerializeField]
	private Text _moneyText;

	// Use this for initialization
	void Start() {
		// 所持金が変化したら表示に反映する
		this.ObserveEveryValueChanged(x => Player.MoneyAmount)
			.DistinctUntilChanged()
			.Subscribe(x => {
				// 表示テキストの更新(3桁ごとにカンマ)
				_moneyText.text = x.ToString("N0");
			});
	}
}