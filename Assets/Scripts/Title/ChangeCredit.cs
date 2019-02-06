using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeCredit : MenuButtonBase {

	[SerializeField]
	private GameObject _creditPanel;

	[SerializeField]
	private GameObject _otherCreditPanel;


	protected override void OnExecute (PointerEventData e) {
		//決定SE
		AudioManager.Instance.PlaySE("Decide",false,1);
		if(_creditPanel.activeSelf == true)
		{
			_otherCreditPanel.SetActive(true);
			_creditPanel.SetActive(false);
		}
		else
		{
			_creditPanel.SetActive(true);
			_otherCreditPanel.SetActive(false);
		}
	
		
	}
}
