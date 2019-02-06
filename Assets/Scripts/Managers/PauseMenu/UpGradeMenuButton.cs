using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;


public class UpGradeMenuButton : MenuButtonBase {

[SerializeField]
private GameObject _upGradeManuPanel;

[SerializeField]
private GameObject _pauseButtons;

	private void Awake() {
	// エスケープキーが押されたら消す
	IObservable<long> escapeStream = Observable
            .EveryUpdate()
            .Where (_ => Input.GetKeyDown(KeyCode.Escape)); 

        escapeStream.Subscribe (_ => {_upGradeManuPanel.SetActive(false);
		_pauseButtons.SetActive(true);}).AddTo(gameObject);
	}

	protected override void OnExecute(PointerEventData e) {
		if(_upGradeManuPanel.activeSelf == true)
		{
		// パネルの非アクティブ化
		_upGradeManuPanel.SetActive(false);
		_pauseButtons.SetActive(true);
		}
		else
		{
		// パネルのアクティブ化
		_upGradeManuPanel.SetActive(true);
		_pauseButtons.SetActive(false);
		}
		AudioManager.Instance.PlaySE("Decide",false,1);

	}
}
