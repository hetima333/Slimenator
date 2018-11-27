using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スキルスロット1つ1つに対して表示の更新を行なう
/// </summary>
[RequireComponent(typeof(SkillSlotCore))]
public class SkillSlotPresenter : MonoBehaviour {

	// スロットの数はコアの静的メンバから取得
	// MARK : 中央、右、左の順
	[SerializeField]
	private SkillImage[] _slots = new SkillImage[SkillSlotCore.SLOT_SIZE];

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
				for (int i = 0; i < SkillSlotCore.SLOT_SIZE; i++) {
					int index = selectedNumber + i;
					if (index >= SkillSlotCore.SLOT_SIZE) {
						index -= SkillSlotCore.SLOT_SIZE;
					}

					if (index >= core.Slot.Count) {
						return;
					}
					_slots[i].ChangeSkillImage(core.Slot[index]);
				}
			});

		core.Slot.ObserveReplace()
			.Where(x => x.NewValue != x.OldValue)
			.Subscribe(x => {
				for (int i = 0; i < SkillSlotCore.SLOT_SIZE; i++) {
					int index = core.SelectedSkillNumber.Value + i;
					if (index >= SkillSlotCore.SLOT_SIZE) {
						index -= SkillSlotCore.SLOT_SIZE;
					}

					if (index >= core.Slot.Count) {
						return;
					}
					_slots[i].ChangeSkillImage(core.Slot[index]);
				}
			});
	}
}