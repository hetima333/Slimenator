using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 終了ボタン
/// </summary>
public class ExitButton : MenuButtonBase {
	protected override void OnExecute(PointerEventData e) {
		// ゲームの終了
		Application.Quit();
	}
}