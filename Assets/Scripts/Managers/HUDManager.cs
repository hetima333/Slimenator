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

	// HPBarへの参照
	private HPBarCore _hpbar;

	// スロットへの参照
	private OrbSlot[] _orbSlots = new OrbSlot[3];

	// 表示される金額のテキスト
	[SerializeField]
	private Text _moneyText;

	void Awake() {
		// プレイヤーへの参照を取得する
		_player = GameObject.Find("Player").GetComponent<EntityPlayer>();
		_hpbar = GameObject.Find("PlayerHPBar").GetComponent<HPBarCore>();
		_orbSlots = GameObject.Find("OrbSlot").GetComponentsInChildren<OrbSlot>();

		// HPBarのターゲットを指定
		_hpbar.Target = _player.GetComponent<IDamageable>();
	}

	// Use this for initialization
	void Start() {
		// 所持金が変化したら表示に反映する
		this.ObserveEveryValueChanged(x => _player.MoneyAmount)
			.DistinctUntilChanged()
			.Subscribe(x => {
				// 表示テキストの更新(3桁ごとにカンマ)
				_moneyText.text = x.ToString("N0");
			});
	}

	void Update() {

		var slot = _player.GetOrbsInSlot().ToArray();
		// オーブのデータを更新する
		for (int i = 0; i < _orbSlots.Length; i++) {

			var orb = Orbs.NONE;
			// キューが大きすぎたら処理を中断
			if (i >= slot.Length) {
				// スロットにデータを代入
				_orbSlots[i].Orb = orb;
				// 次以降のスロット全ても同じ
				continue;
			}

			// 属性ごとの分岐
			switch (slot[i].name) {
				case "Fire":
					orb = Orbs.FIRE;
					break;
				case "Ice":
					orb = Orbs.WATER;
					break;
				case "Lightning":
					orb = Orbs.THUNDER;
					break;
			}
			// スロットにデータを代入
			_orbSlots[i].Orb = orb;
		}

	}
}