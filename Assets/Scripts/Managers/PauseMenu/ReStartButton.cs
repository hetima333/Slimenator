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
		//Tutorialシーンでは専用HUDを使用するのでプレイシーンのみHUD差異読み込む
		if (SceneManager.GetActiveScene().name == "PlayTestScene") {
					
					SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
				}
		
		// ポーズ解除
		if(Pausable.Instance!=null)
		Pausable.Instance.Pausing = false;
	}
}