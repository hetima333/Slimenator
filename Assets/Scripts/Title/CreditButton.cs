using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreditButton : MenuButtonBase {

	[SerializeField]
	private GameObject _creditPanel;

	protected override void OnExecute (PointerEventData e) {
		//決定SE
		AudioManager.Instance.PlaySE("Decide",false,1);
		if(_creditPanel.activeSelf == true)
		{
			_creditPanel.SetActive(false);
		}
		else
		{
			_creditPanel.SetActive(true);
		}
	
		
	}
}
