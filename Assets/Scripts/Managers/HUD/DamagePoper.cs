using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoper : SingletonMonoBehaviour<DamagePoper> {

	[SerializeField]
	private DamageText _dmgText;

	/// <summary>
	/// ダメージをポップする
	/// </summary>
	/// <param name="worldPosition">ポップするワールド座標</param>
	/// <param name="damage">ダメージ</param>
	public void PopDamage(Vector3 worldPosition, int damage){
		// ワールド座標をスクリーン座標に変換する
		Vector3 pos = Camera.main.WorldToScreenPoint(worldPosition);

		// TODO : オブジェクトプールを使ってインスタンス化する
		// var ins = Instantiate(_dmgText, pos, Quaternion.identity);
		var ins = ObjectManager.Instance.InstantiateWithObjectPooling(_dmgText.gameObject, pos);
		// 親子関係を設定
		ins.transform.SetParent(this.transform);

		// ダメージをセットする
		ins.GetComponent<DamageText>().SetDamage(damage);

	}
}