using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HPBarCore : MonoBehaviour {

	[SerializeField]
	private Slider _slider;
	// 減少中のHP
	private float _redHP;

	// HP減少の速度
	[SerializeField, Range(0.01f, 2.0f)]
	private float _hpDecreaseSpeed;

	private RectTransform _redHPBarTrans;
	private float _maxWidth;

	// Use this for initialization
	void Start () {
		var dmg = GetComponent<IDamageable>();

		_slider = GetComponent<Slider>();
		_slider.minValue = 0;
		_redHP = dmg.HipPoint;
		_redHPBarTrans = _slider.GetComponentInChildren<Image>().rectTransform;
		_maxWidth = _redHPBarTrans.sizeDelta.x;

		// 現在のHPの変化を監視する
		dmg.ObserveEveryValueChanged(x => dmg.HipPoint)
			.Subscribe(x => {
				_slider.value = x;
				// 赤HPが緑HPより少なかったら緑HPの位置に合わせる
				if(_redHP < x){
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
	void Update () {
		// 現HPより赤HPが多ければ赤HPをへらす
		if(_redHP > _slider.value){
			_redHP -= _hpDecreaseSpeed;
		}
	}
}
