using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameTargetPresenter : MonoBehaviour {

	[SerializeField]
	private Image _panel;

	[SerializeField]
	private Text _targetText;

	[SerializeField]
	private GameObject _targetImages;

	private float _defaultWidth = 750.0f;

	// Use this for initialization
	void Start () {
		_panel.rectTransform.sizeDelta = new Vector2(_defaultWidth, _panel.rectTransform.sizeDelta.y);
		_targetText.gameObject.SetActive(false);


		this.ObserveEveryValueChanged(_ => GameStateManager.Instance._norm)
			.Where(x => x <= 0)
			.Subscribe(_ => {
				_panel.rectTransform.sizeDelta = new Vector2(1000.0f, _panel.rectTransform.sizeDelta.y);
				_targetText.gameObject.SetActive(true);
				_targetImages.gameObject.SetActive(false);
			});

		GameStateManager.Instance.IsBossEnable
			.Where(x => x)
			.Subscribe(x => {
				if(Language.Instance.language == "日本語")
				{
					_targetText.text = "ボスを倒せ！";
				}
				else{
					_targetText.text = "Kill boss";
				}
				
			});
	}
}
