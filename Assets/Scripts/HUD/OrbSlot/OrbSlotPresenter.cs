using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// オーブスロット1つ1つに対して表示の更新を行なう
/// </summary>
[RequireComponent(typeof(OrbSlotCore))]
public class OrbSlotPresenter : MonoBehaviour {

	void Start() {
		var core = GetComponent<OrbSlotCore>();
		var slots = GetComponentsInChildren<OrbSlotIndicater>();

		if (slots == null) {
			Debug.LogError("OrbSlotIndicaterが存在しません\nOrbSlotIndicater is not found.", this.gameObject);
		} else {
			// スロットの変更を元に表示を更新する
			core.Slot.ObserveReplace()
				.Where(x => x.NewValue != x.OldValue)
				.Subscribe(x => {
					slots[x.Index].ChangeOrbImage(x.NewValue);
				});

		}
	}
}