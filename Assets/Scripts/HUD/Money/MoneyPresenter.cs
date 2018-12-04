using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MoneyCore))]
public class MoneyPresenter : MonoBehaviour {
	// 自身のRectTransform
	private RectTransform _rectTrans;

	// 初期座標
	private Vector3 _defaultPosition;

	// 移動量
	private float _moveAmount;

	// 移動にかかる時間
	[SerializeField, Range(0.0f, 5.0f)]
	private float _duration = 0.75f;

	// 進行方向フラグ
	[SerializeField]
	private bool _isShow;

	// 停止時間
	private float _stopDuration = 0.75f;

	// 表示される金額のテキスト
	[SerializeField]
	private Text _moneyText;

	void Start() {
		_rectTrans = GetComponent<RectTransform>();
		_defaultPosition = _rectTrans.anchoredPosition;
		_moveAmount = _rectTrans.sizeDelta.y;
		_rectTrans.anchoredPosition = new Vector2(_rectTrans.anchoredPosition.x, _moveAmount);

		var core = GetComponent<MoneyCore>();

		// 所持金が変動したら
		core.ObserveEveryValueChanged(x => x.MoneyAmount)
			.Skip(1)
			.DistinctUntilChanged()
			.Subscribe(x => {
				// 表示テキストの更新(3桁ごとにカンマ)
				_moneyText.text = x.ToString("N0");

				// 表示する
				_isShow = true;
			});

		// 表示非表示切り替えコルーチンの開始
		StartCoroutine(ShowAndHide());
	}

	IEnumerator ShowAndHide() {
		while (true) {
			// 移動量の計算
			var amount = Vector2.up * (_moveAmount / _duration) * Time.deltaTime;

			// 見える
			if (_isShow) {
				if (_rectTrans.anchoredPosition.y > _defaultPosition.y) {
					_rectTrans.anchoredPosition -= amount;
					// 越えたら数値を整える
					if (_rectTrans.anchoredPosition.y < _defaultPosition.y) {
						_rectTrans.anchoredPosition = new Vector2(_rectTrans.anchoredPosition.x, _defaultPosition.y);
						yield return new WaitForSeconds(_stopDuration);
						_isShow = false;
					}
				}
			}

			// 隠れる
			else {
				if (_rectTrans.anchoredPosition.y < _moveAmount) {
					_rectTrans.anchoredPosition += amount;
					// 越えたら数値を整える
					if (_rectTrans.anchoredPosition.y > _moveAmount) {
						_rectTrans.anchoredPosition = new Vector2(_rectTrans.anchoredPosition.x, _moveAmount);
					}
				}
			}

			// 次フレームまで待機
			yield return null;
		}
	}
}