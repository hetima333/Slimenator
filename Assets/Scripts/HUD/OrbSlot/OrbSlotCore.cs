using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// オーブスロットのコア
/// </summary>
public class OrbSlotCore : MonoBehaviour {

	// オーブスロットの大きさ
	public static readonly int SLOT_SIZE = 3;

	// スロットのリアクティブコレクション
	private ReactiveCollection<Orbs> _slot = new ReactiveCollection<Orbs>();
	public IReadOnlyReactiveCollection<Orbs> Slot {
		get { return _slot; }
	}

	// HUDManagerを一時的にシリアライズ
	[SerializeField]
	private HUDManager _manager;

	void Start() {
		var player = _manager.Player;

		// スロットの初期化
		for (int i = 0; i < SLOT_SIZE; i++) {
			_slot.Add(Orbs.NONE);
		}

		// Update毎にスロット内の情報を更新
		this.UpdateAsObservable()
			.Subscribe(_ => {
				var orbs = player.GetOrbsInSlot()
					.Select(x => ElementTypeToOrbs(x))
					.ToArray();

				// スロット1つ1つに対して処理
				for (int i = 0; i < SLOT_SIZE; i++) {
					// オーブがスロットより少なかったら空
					_slot[i] = i >= orbs.Length ? Orbs.NONE : orbs[i];
				}
			});
	}

	/// <summary>
	/// ElementTypeからオーブ型への変換
	/// </summary>
	private Orbs ElementTypeToOrbs(ElementType element) {
		var result = Orbs.NONE;

		// 名前で分類
		switch (element.name) {
			case "Fire":
				result = Orbs.FIRE;
				break;
			case "Ice":
				result = Orbs.ICE;
				break;
			case "Lightning":
				result = Orbs.LIGHTNING;
				break;
		}

		return result;
	}
}