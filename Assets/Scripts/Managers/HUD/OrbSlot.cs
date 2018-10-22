using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbSlot : MonoBehaviour {

	private Orbs _orb = Orbs.NONE;
	public Orbs Orb{
		get { return _orb; }
		set	{
			if(_orb == value){
				return;
			}
			_orb = value;
			// ここから(画像の変更)
			ChangeOrbImage(value);
		}
	}

	private Image _image;

	// Use this for initialization
	void Start () {
		_image = GetComponent<Image>();
	}

	private void ChangeOrbImage(Orbs orb){
		Sprite sprite = null;
		switch(orb){
			case Orbs.FIRE:
				sprite = OrbDefine.Instance.fireSprite;
				break;
			case Orbs.WATER:
				sprite = OrbDefine.Instance.waterSprite;
				break;
			case Orbs.THUNDER:
				sprite = OrbDefine.Instance.thunderSprite;
				break;
		}

		// スプライトを設定
		_image.sprite = sprite;

		// スプライトが設定されなければ透明にする
		if(sprite == null){
			_image.color = _image.color * new Color(1, 1, 1, 0);
		}
		else{
			_image.color = Color.white;
		}

	}
}
