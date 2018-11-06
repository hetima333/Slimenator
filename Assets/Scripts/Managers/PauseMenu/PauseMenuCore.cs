using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuCore : MonoBehaviour {

	// 仮のキー配置
	// TODO : コントローラーからも取得できるように
	[SerializeField]
	private KeyCode _pauseKey = KeyCode.Escape;

	void Start() {
		var group = GetComponentInChildren<CanvasGroup>();

		// ポーズ状態が変化したらパネルの表示・非表示を切り替える
		Pausable.Instance.ObserveEveryValueChanged(x => x.Pausing)
			.Subscribe(x => {
				// 透明度の変更
				group.alpha = x ? 1.0f : 0.0f;
				// ボタン等の有効・無効
				group.interactable = x;
				// レイキャストの有効・無効
				group.blocksRaycasts = x;
			});
	}

	void Update() {
		// ポーズのトグル
		if (Input.GetKeyDown(_pauseKey)) {
			Pausable.Instance.Pausing = !Pausable.Instance.Pausing;
		}
	}
}