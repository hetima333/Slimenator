using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MoneyCore))]
public class MoneyPresenter : MonoBehaviour {


	private float _startY;
	private float _endY;

	// シークエンス
	private Sequence _sequence;

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
		var core = GetComponent<MoneyCore>();
		var rectTrans = GetComponent<RectTransform>();

		// ゴール座標の設定
		_startY = rectTrans.anchoredPosition.y;
		_endY = rectTrans.sizeDelta.y;

		// コイン取得時の効果
		_sequence = DOTween.Sequence();
		_sequence.Append(rectTrans.DOAnchorPosY(_startY, _duration));
		_sequence.AppendInterval(_duration);
		_sequence.Append(rectTrans.DOAnchorPosY(_endY, _duration));
		_sequence.SetAutoKill(false);

		_sequence.Pause();

		// 初期座標の設定
		rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, rectTrans.sizeDelta.y);

		// 所持金が変動したら
		core.ObserveEveryValueChanged(x => x.MoneyAmount)
			.Skip(1)
			.DistinctUntilChanged()
			.Subscribe(x => {
				// 表示テキストの更新(3桁ごとにカンマ)
				_moneyText.text = x.ToString("N0");

				_sequence.Restart();
			});
	}
}