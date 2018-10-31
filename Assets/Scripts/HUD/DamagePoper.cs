using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoper : SingletonMonoBehaviour<DamagePoper> {

	[SerializeField]
	private DamageText _dmgText;

	/// <summary>
	/// ダメージをポップする
	/// </summary>
	/// <param name="trans">ポップする座標</param>
	/// <param name="damage">ダメージ</param>
	public void PopDamage(Transform trans, int damage) {
		if (damage == 0) {
			return;
		}
		// TODO : オブジェクトプールを使ってインスタンス化する
		var ins = ObjectManager.Instance.InstantiateWithObjectPooling(_dmgText.gameObject);
		// 親子関係を設定
		ins.transform.SetParent(trans);
		ins.transform.localPosition = Vector3.zero;
		ins.transform.localScale = Vector3.one;

		// ダメージをセットする
		ins.GetComponent<DamageText>().SetDamage(damage);

	}
}