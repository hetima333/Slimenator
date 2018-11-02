using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// オーブスロット1つ1つに対して表示の更新を行なう
/// </summary>
[RequireComponent(typeof(OrbSlotCore))]
public class OrbSlotPresenter : MonoBehaviour {

	// スロットの数はコアの静的メンバから取得
	[SerializeField]
	private Image[] _slots = new Image[OrbSlotCore.SLOT_SIZE];

	[SerializeField]
	private OrbSprites _sprites;

	void Start() {
		var core = GetComponent<OrbSlotCore>();

		if (_slots == null) {
			Debug.LogError("オーブスロットが存在しません\nOrbSlot is not found.", this.gameObject);
		} else if (_slots.Length < OrbSlotCore.SLOT_SIZE) {
			Debug.LogError("オーブスロットの数が足りません\nOrbSlot is not enough.", this.gameObject);
		} else if (_sprites == null) {
			Debug.LogError("オーブ画像が設定されていません\nOrbSprite is not found.", this.gameObject);
		} else {
			// スロットの変更を元に表示を更新する
			core.Slot.ObserveReplace()
				.Where(x => x.NewValue != x.OldValue)
				.Subscribe(x => {
					var slot = _slots[x.Index];
					// 該当の画像の読み込み
					slot.sprite = OrbsToSprite(x.NewValue);

					// スプライトが設定されなければ透明にする
					if (slot.sprite == null) {
						slot.color = slot.color * new Color(1, 1, 1, 0);
					}
					// スプライトが設定されていたら不透明にする
					else {
						slot.color = Color.white;
					}
				});
		}
	}

	/// <summary>
	/// オーブから対応するスプライトへの変換
	/// </summary>
	Sprite OrbsToSprite(Orbs orb) {
		Sprite sprite;
		switch (orb) {
			case Orbs.FIRE:
				sprite = _sprites.Fire;
				break;
			case Orbs.ICE:
				sprite = _sprites.Ice;
				break;
			case Orbs.LIGHTNING:
				sprite = _sprites.Lightning;
				break;
			default:
				sprite = null;
				break;
		}

		return sprite;
	}
}