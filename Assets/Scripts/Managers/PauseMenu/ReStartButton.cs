using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// リスタートボタン
/// </summary>
public class ReStartButton : MenuButtonBase {
	protected override void OnExecute(PointerEventData e) {
		// 現在のシーンのリロード
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
		// ポーズ解除
		Pausable.Instance.Pausing = false;
	}
}