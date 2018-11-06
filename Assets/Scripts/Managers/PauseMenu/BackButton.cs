using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 戻るボタン
/// </summary>
public class BackButton : MenuButtonBase {
	protected override void OnExecute(PointerEventData e) {
		// ゲームの再開
		Pausable.Instance.Pausing = false;
	}
}