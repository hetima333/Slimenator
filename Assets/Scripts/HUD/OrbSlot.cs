using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbSlot : MonoBehaviour {

	private Orbs _orb = Orbs.NONE;
	public Orbs Orb {
		get { return _orb; }
		set {
			if (_orb == value) {
				return;
			}
			_orb = value;
			// ここから(画像の変更)
			ChangeOrbImage(value);
		}
	}

	private Image _image;
	[SerializeField]
	private OrbSprites _sprites;

	// Use this for initialization
	void Start() {
		_image = GetComponent<Image>();
		// オーブ画像の読み込み
		_sprites = Resources.Load("HUD/OrbSprites", typeof(OrbSprites))as OrbSprites;

		// 最初はスロットを空にする
		ChangeOrbImage(Orbs.NONE);
	}

	/// <summary>
	/// オーブの種類によって画像を変更する
	/// </summary>
	/// <param name="orb"></param>
	private void ChangeOrbImage(Orbs orb) {
		Sprite sprite = null;
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
		}

		// スプライトが設定されなければ透明にする
		if (sprite == null) {
			_image.color = _image.color * new Color(1, 1, 1, 0);
		} else {
			// スプライトを設定
			_image.sprite = sprite;
			_image.color = Color.white;
		}

	}
}