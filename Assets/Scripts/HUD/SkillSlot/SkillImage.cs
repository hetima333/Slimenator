using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillImage : MonoBehaviour {

	[SerializeField]
	private Image _baseElementImage;
	[SerializeField]
	private Text _baseTierText;
	[SerializeField]
	private Image _enchantmentImage;
	[SerializeField]
	private Text _enchantmentTierText;

	[SerializeField]
	private OrbSprites _sprites;

	public void ChangeSkillImage(Skill skill) {
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

			// MARK : 現在ユニークスキル以外はエンチャント1つなのでこの書き方にしている
			var enchantmentOrb = skill.GetStatusEffects() [0].ToOrb();
			_enchantmentImage.sprite = OrbToSprite(enchantmentOrb);
			_enchantmentTierText.text = enchantmentOrb == Orb.NONE ? "" : skill.GetEnchantmentTier().ToString();
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