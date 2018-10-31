using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class OrbSlotPresenter : MonoBehaviour {

	void Start() {
		var core = GetComponent<OrbSlotCore>();
		var slots = GetComponentsInChildren<OrbSlotView>();

		// スロットの変更を元に表示を更新する
		core.Slot.ObserveReplace()
			.Where(x => x.NewValue != x.OldValue)
			.Subscribe(x => {
				slots[x.Index].ChangeOrbImage(x.NewValue);
			});
	}
}