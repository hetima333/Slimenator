using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
	private Animator[] _slotAnimators = new Animator[OrbSlotCore.SLOT_SIZE];

	[SerializeField]
	private OrbSprites _sprites;

	private SkillImage _skillImage;

	void Start() {
		var core = GetComponent<OrbSlotCore>();

		_skillImage = GetComponentInChildren<SkillImage>();

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
					var animator = _slotAnimators[x.Index];
					animator.SetInteger("OrbType", (int) x.NewValue);

					var trans = animator.transform;
					trans.localScale = Vector3.zero;
					trans.DOScale(Vector3.one, 0.2f);
				});

			// 生成されるスキルが更新されたら
			core.ExpectedSkill
				.Subscribe(skill => {
					_skillImage.ChangeSkillImage(skill);
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