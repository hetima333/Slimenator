﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	[SerializeField, Header("ExpectedSkill")]
	private Image _baseElementImage;
	[SerializeField]
	private Text _baseTierText;
	[SerializeField]
	private Image _enchantmentImage;
	[SerializeField]
	private Text _enchantmentTierText;

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
					slot.sprite = OrbToSprite(x.NewValue);

					// スプライトが設定されなければ透明にする
					if (slot.sprite == null) {
						slot.color = slot.color * new Color(1, 1, 1, 0);
					}
					// スプライトが設定されていたら不透明にする
					else {
						slot.color = Color.white;
					}
				});

			// 生成されるスキルが更新されたら
			core.ExpectedSkill
				.Subscribe(skill => {
					// スキルがなければSpriteを空にする
					if (skill == null) {
						_baseElementImage.sprite = null;
						_baseTierText.text = "";
						_enchantmentImage.sprite = null;
						_enchantmentTierText.text = "";
					}
					// ユニークスキル
					else if (skill.IsUnique()) {
						// TODO : スキル画像が来たら変更する
						_baseElementImage.sprite = null;
						_baseTierText.text = "unique";
						_enchantmentImage.sprite = null;
						_enchantmentTierText.text = "";
					}
					// ユニークでないスキル
					else {
						var tier = skill.GetSkillTier().GetMultiplyer();
						var baseOrb = skill.ToOrb().First();

						_baseElementImage.sprite = OrbToSprite(baseOrb);
						_baseTierText.text = tier.ToString();

						var enchantmentOrb = core.Slot.FirstOrDefault(x => x != baseOrb);
						_enchantmentImage.sprite = OrbToSprite(enchantmentOrb);
						if (enchantmentOrb == Orb.NONE) {
							_enchantmentTierText.text = "";
						} else {
							_enchantmentTierText.text = core.Slot.Skip(1).TakeWhile(x => x == enchantmentOrb).Count().ToString();
						}
					}
				});

		}

	}

	/// <summary>
	/// オーブから対応するスプライトへの変換
	/// </summary>
	Sprite OrbToSprite(Orb orb) {
		Sprite sprite;
		switch (orb) {
			case Orb.FIRE:
				sprite = _sprites.Fire;
				break;
			case Orb.ICE:
				sprite = _sprites.Ice;
				break;
			case Orb.LIGHTNING:
				sprite = _sprites.Lightning;
				break;
			default:
				sprite = null;
				break;
		}

		return sprite;
	}
}