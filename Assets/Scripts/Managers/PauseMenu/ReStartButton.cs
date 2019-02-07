using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// リスタートボタン
/// </summary>
public class ReStartButton : MenuButtonBase {
	protected override void OnExecute(PointerEventData e) {
        // 現在のシーンのリロード
        FadeManager.Instance.StartTransition(1.0f, SceneManager.GetActiveScene().name);

		// ポーズ解除
		if(Pausable.Instance!=null)
		Pausable.Instance.Pausing = false;
	}
}