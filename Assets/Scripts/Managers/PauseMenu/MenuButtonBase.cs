using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuButtonBase : MonoBehaviour {

	private void Start() {
		// ボタンの参照を取得
		var button = GetComponent<Button>();

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
				// TODO : 色を変えるなどの効果
			});

		// フォーカスが外れた時
		button.OnDeselectAsObservable()
			.Subscribe(e => {
				// TODO : フォーカス時の効果削除
			});

		// 項目が決定された時
		// TODO : 別途コントローラー用の設定も必要？
		button.OnPointerClickAsObservable()
			.Subscribe(e => {
				OnExecute(e);
			});
	}

	/// <summary>
	/// ボタン内の動作を実行した時に呼ばれる
	/// </summary>
	protected virtual void OnExecute(PointerEventData e) {

	}
}