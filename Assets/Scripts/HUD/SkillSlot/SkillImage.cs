﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillImage : MonoBehaviour {

	[SerializeField]
	private Image _baseElementImage;
	[SerializeField]
	private Image _enchantmentImage;
	[SerializeField]
	private Image _enchantmentBackImage;

	[SerializeField]
	private OrbSprites _sprites;

	void Start() {
		// _enchantmentBackImage = _enchantmentImage.transform.parent.GetComponent<Image>();

		Debug.Log("come : " + _enchantmentBackImage.name);
	}

	public void ChangeSkillImage(Skill skill) {
		// スキルがあれば不透明にする
		if (skill != null) {
			_baseElementImage.color = new Color(1, 1, 1, 1);
			_enchantmentImage.color = new Color(1, 1, 1, 1);
			_enchantmentBackImage.color = new Color(1, 1, 1, 1);
		}

		// スキルがなければSpriteを空にする
		if (skill == null) {
			_baseElementImage.color = new Color(1, 1, 1, 0);
			_enchantmentImage.color = new Color(1, 1, 1, 0);
			_enchantmentBackImage.color = new Color(1, 1, 1, 0);
		}
		// ユニークスキル
		else if (skill.IsUnique()) {
			_enchantmentImage.color = new Color(1, 1, 1, 0);
			_enchantmentBackImage.color = new Color(1, 1, 1, 0);
			_baseElementImage.sprite = TextureLoader.Load("HUD/SkillUI/" + skill.name);
		}
		// ユニークでないスキル
		else {
			var tier = skill.GetSkillTier().GetMultiplyer();
			var baseOrb = skill.GetBaseElement().name;

			_baseElementImage.sprite = TextureLoader.Load("HUD/SkillsUI/" + baseOrb + tier);

			// MARK : 現在ユニークスキル以外はエンチャント1つなのでこの書き方にしている
			var effects = skill.GetStatusEffects();
			var enchantmentOrb = effects.Count > 0 ? effects[0].ToOrb() : Orb.NONE;
			_enchantmentImage.sprite = OrbToSprite(enchantmentOrb);
			// _enchantmentTierText.text = enchantmentOrb == Orb.NONE ? "" : skill.GetEnchantmentTier().ToString();
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