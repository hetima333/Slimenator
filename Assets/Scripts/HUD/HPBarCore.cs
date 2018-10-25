using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HPBarCore : MonoBehaviour {

	private Slider _slider;
	// 減少中のHP
	private float _redHP;

	// HP減少の速度
	[SerializeField, Range(0.3f, 2.0f)]
	private float _hpDecreaseSpeed = 0.5f;

	[SerializeField]
	private RectTransform _redHPBarTrans;
	private float _maxWidth;

	public IDamageable Target{
		get; set;
	}

	// Use this for initialization
	void Start() {
		if(Target == null){
			Target = GetComponent<IDamageable>();
		}

		_slider = GetComponent<Slider>();
		_slider.minValue = 0;
		_slider.maxValue = Target.MaxHitPoint;
		_redHP = Target.HitPoint;
		_maxWidth = _redHPBarTrans.sizeDelta.x;

		// 現在のHPの変化を監視する
		Target.ObserveEveryValueChanged(x => Target.HitPoint)
			.Subscribe(x => {
				_slider.value = x;
				// 赤HPが緑HPより少なかったら緑HPの位置に合わせる
				if (_redHP < x) {
					_redHP = x;
				}
			});

		// 赤HPの変化を監視する
		this.ObserveEveryValueChanged(x => _redHP)
			.Subscribe(x => {
				// HPバーにサイズを適用する
				_redHPBarTrans.sizeDelta = new Vector2(_redHP * _maxWidth / _slider.maxValue, _redHPBarTrans.sizeDelta.y);
			});
	}

	// Update is called once per frame
	void Update() {
		// 現HPより赤HPが多ければ赤HPをへらす
		if (_redHP > _slider.value) {
			_redHP -= _hpDecreaseSpeed;
		}
	}
}