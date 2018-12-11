using System.Collections;
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
			_baseElementImage.sprite = TextureLoader.Load("HUD/SkillsUI/" + skill.name);
		}
		// ユニークでないスキル
		else {
			var tier = skill.GetSkillTier().GetMultiplyer();
			var baseOrb = skill.GetBaseElement().name;

			_baseElementImage.sprite = TextureLoader.Load("HUD/SkillsUI/" + baseOrb + tier);

			// MARK : 現在ユニークスキル以外はエンチャント1つなのでこの書き方にしている
			var effects = skill.GetStatusEffects();
			var enchantmentOrb = effects.Count > 1 ? effects[1].ToOrb() : Orb.NONE;
			_enchantmentImage.sprite = OrbToSprite(enchantmentOrb);

			// エンチャントがなければ透明にする
			if (enchantmentOrb == Orb.NONE) {
				_enchantmentImage.color = new Color(1, 1, 1, 0);
				_enchantmentBackImage.color = new Color(1, 1, 1, 0);
			} else {
				_enchantmentImage.color = new Color(1, 1, 1, 1);
				_enchantmentBackImage.color = new Color(1, 1, 1, 1);
			}
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