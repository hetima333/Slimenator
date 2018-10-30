using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MenuButtonBase {

	protected override void OnExecute(PointerEventData e) {
		// ゲームの再開
		Pausable.Instance.Pausing = false;
	}
}