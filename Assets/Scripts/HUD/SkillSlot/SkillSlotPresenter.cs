using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スキルスロット1つ1つに対して表示の更新を行なう
/// </summary>
[RequireComponent(typeof(SkillSlotCore))]
public class SkillSlotPresenter : MonoBehaviour {

	// スロットの数はコアの静的メンバから取得
	[SerializeField]
	private SkillImage[] _slots = new SkillImage[SkillSlotCore.SLOT_SIZE];

	// 選択スキルを示す枠
	[SerializeField]
	private Image _selectedImage;

	// スキルが選択された時に拡大するスケール
	private float _imaegPopScale = 1.3f;

	void Start() {
		var core = GetComponent<SkillSlotCore>();

		// スロットの初期化
		foreach (var slot in _slots) {
			slot.ChangeSkillImage(null);
		}

		// 選択中スキルが変更されたら
		core.SelectedSkillNumber
			.DistinctUntilChanged()
			.Subscribe(selectedNumber => {
				// 選択スキル番号の位置に枠を表示する
				_selectedImage.rectTransform.DOAnchorPosX(45.0f + selectedNumber * 165.0f, 0.2f);

				// 選択スキル番号のスキル画像をちょっと上に上げる
				for (int i = 0; i < SkillSlotCore.SLOT_SIZE; i++) {
					if (i == selectedNumber) {
						_slots[i].transform.DOScale(_imaegPopScale, 0.2f);
					} else {
						_slots[i].transform.DOScale(1.0f, 0.2f);
					}

				}
			});

		core.Slot.ObserveReplace()
			.Where(x => x.NewValue != x.OldValue)
			.Subscribe(x => {
				// 変更されたスキルの画像を更新
				_slots[x.Index].ChangeSkillImage(x.NewValue);
			});
	}
}