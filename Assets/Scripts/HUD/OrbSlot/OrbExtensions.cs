using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Orb {
	NONE = 0,
	FIRE,
	ICE,
	LIGHTNING
}

public static class OrbExtensions {

	/// <summary>
	/// ElementTypeからオーブ型への変換
	/// </summary>
	public static Orb ToOrb(this ElementType self) {
		// 名前で分類
		switch (self.name) {
			case "Fire":
				return Orb.FIRE;
			case "Ice":
				return Orb.ICE;
			case "Lightning":
				return Orb.LIGHTNING;
		}

		return Orb.NONE;
	}

	/// <summary>
	/// ユニークスキルかどうか
	/// </summary>
	public static bool IsUnique(this Skill self) {
		return self.GetBaseElement() == null;
	}

	/// <summary>
	/// スキルからオーブ(IEnumerable)への変換
	/// </summary>
	public static IEnumerable<Orb> ToOrb(this Skill self) {
		// ユニークスキルの場合はコンビネーションエレメントから取得する
		if (self.IsUnique()) {
			return self.GetCombinationElements().Select(x => x.ToOrb());
		}
		// ユニークでない場合はベースエレメントをTier分だけ積む
		else {
			var orb = self.GetBaseElement().ToOrb();
			int tier = self.GetSkillTier().GetMultiplyer();
			return Enumerable.Repeat<Orb>(orb, tier);
		}
	}
}