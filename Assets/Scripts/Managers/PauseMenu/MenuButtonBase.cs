using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuButtonBase : MonoBehaviour {

	private PauseMenuCore _menuCore;
	protected PauseMenuCore MenuCore {
		get { return _menuCore; }
	}

	private void Start() {
		// ボタンの参照を取得
		var button = GetComponent<Button>();

		// メニューコアの参照取得
		_menuCore = GetComponentInParent<PauseMenuCore>();

		// ポインターが入った時にフォーカスをあわせる
		button.OnPointerEnterAsObservable()
			.Subscribe(_ => {
				EventSystem.current.SetSelectedGameObject(this.gameObject);
			});

		// ポインターが外れた時にフォーカスを外す
		button.OnPointerExitAsObservable()
			.Subscribe(_ => {
				EventSystem.current.SetSelectedGameObject(null);
			});

		// フォーカスが入った時
		button.OnSelectAsObservable()
			.Subscribe(e => {
				Debug.Log("Select : " + this.gameObject.name);
				// TODO : 色を変えるなどの効果
			});

		// フォーカスが外れた時
		button.OnDeselectAsObservable()
			.Subscribe(e => {
				Debug.Log("DeSelect : " + this.gameObject.name);
				// TODO : フォーカス時の効果削除
			});

		// 項目が決定された時
		// TODO : 別途コントローラー用の設定も必要？
		button.OnPointerClickAsObservable()
			.Subscribe(e => {
				Debug.Log("Execute : " + this.gameObject.name);
				OnExecute(e);
			});
	}

	/// <summary>
	/// ボタン内の動作を実行した時に呼ばれる
	/// </summary>
	protected virtual void OnExecute(PointerEventData e) {

	}
}