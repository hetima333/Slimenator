using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrbDefine {

	/// <summary>
	/// ElementTypeからオーブ型への変換
	/// </summary>
	public static Orbs ToOrbs(this ElementType element) {
		// 名前で分類
		switch (element.name) {
			case "Fire"		: return Orbs.FIRE;
			case "Ice"		: return Orbs.ICE;
			case "Lightning": return Orbs.LIGHTNING;
		}

		return 0;
	}

}
