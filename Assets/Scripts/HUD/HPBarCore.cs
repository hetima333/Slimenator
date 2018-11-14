using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HPBarCore : MonoBehaviour {

	private Slider _slider;
	// 敵の現在HP
	private FloatReactiveProperty _greenHP = new FloatReactiveProperty();
	// 減少中のHP
	private FloatReactiveProperty _redHP = new FloatReactiveProperty();
	// HPの参照元
	private IDamageable _target;

	// HP減少の速度
	[SerializeField, Range(0.1f, 2.0f)]
	private float _hpDecreaseSpeed = 0.5f;

	[SerializeField, Range(0.0f, 5.0f)]
	private float _overLifeTime = 0.2f;

	[SerializeField]
	private RectTransform _redHPBarTrans;
	private float _maxWidth;

	private bool _initFlg = false;

	void InitAtOnce() {
		// 初期化は一度だけ行なう
		if (_initFlg) {
			return;
		}

		_maxWidth = _redHPBarTrans.sizeDelta.x;

		// 緑HPの変化の監視
		_greenHP
			.DistinctUntilChanged()
			.Subscribe(x => {
				// ダメージのポップ
				var dmg = _slider.value - x;
				// ダメージが1以上なら表示する
				if (dmg > 0) {
					DamagePoper.Instance.PopDamage(transform.parent, (int)dmg);
				}

				_slider.value = x;

				// 赤HPが緑HPより少なかったら緑HPに合わせる
				if (_redHP.Value < x) {
					_redHP.Value = x;
				}
			});

		// 赤HPの変化の監視
		_redHP
			.DistinctUntilChanged()
			.Subscribe(x => {
				// HPバーにサイズを適用する
				var size = new Vector2(_maxWidth * (x / _slider.maxValue), _redHPBarTrans.sizeDelta.y);
				// 最大サイズを越えないようにする
				if (size.x > _maxWidth) {
					size.x = _maxWidth;
				}
				_redHPBarTrans.sizeDelta = size;
			});

		// 赤HPがなくなったら非表示にする
		_redHP
			.Where(x => x <= 0)
			// ディレイをかける
			.Delay(TimeSpan.FromSeconds(_overLifeTime))
			.Subscribe(_ => {
				IsShow(false);
			});

		_initFlg = true;
	}

	// Update is called once per frame
	void Update() {
		// 初期化されていなければ処理を行わない
		if (_initFlg != true) {
			return;
		}

		// HPの更新を通知
		_greenHP.SetValueAndForceNotify(_target.HitPoint);

		// 現HPより赤HPが多ければ赤HPをへらす
		if (_redHP.Value > _slider.value) {
			_redHP.SetValueAndForceNotify(_redHP.Value - _hpDecreaseSpeed);
		}
	}

	public void Init(IDamageable damageable) {
		// スライダーの取得
		_slider = GetComponent<Slider>();
		IsShow(true);

		// ターゲットの設定
		_target = damageable;

		// HPの設定
		_greenHP.Value = _target.HitPoint;

		// 赤HPをターゲットのHPとする
		_redHP.Value = _target.HitPoint;

		_slider.minValue = 0;
		_slider.maxValue = _target.MaxHitPoint;

		// 一度だけの初期化	
		InitAtOnce();
	}

	/// <summary>
	/// 表示・非表示
	/// </summary>
	void IsShow(bool flg) {
		transform.GetComponentInParent<CanvasGroup>().alpha = flg ? 1.0f : 0.0f;
	}
}