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
	[SerializeField]
	private SkillImage[] _slots = new SkillImage[SkillSlotCore.SLOT_SIZE];

	void Start() {
		var core = GetComponent<SkillSlotCore>();

		_slots = GetComponentsInChildren<SkillImage>();

		// スロットの初期化
		foreach (var slot in _slots) {
			slot.ChangeSkillImage(null);
		}

		// 選択中スキルが変更されたら
		core.SelectedSkillNumber
			// .DistinctUntilChanged()
			.Subscribe(selectedNumber => {
				// 該当スキルの拡大率を変更する
				var slotsTrans = _slots
					.Select(slot => slot.GetComponent<RectTransform>())
					.Select((Value, Index) => new { Value, Index });

				// 拡大率の変更
				foreach (var slot in slotsTrans) {
					if (slot.Index == selectedNumber) {
						slot.Value.localScale = Vector3.one * 1.2f;
					} else {
						slot.Value.localScale = Vector3.one;
					}
				}
			});

		core.Slot.ObserveReplace()
			.Where(x => x.NewValue != x.OldValue)
			.Subscribe(x => {
				_slots[x.Index].ChangeSkillImage(x.NewValue);
			});
	}
}