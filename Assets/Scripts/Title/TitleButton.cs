using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleButton : MenuButtonBase {

	[SerializeField]
	private string _sceneName;

	protected override void OnExecute (PointerEventData e) {
		//決定SE
		AudioManager.Instance.PlaySE("Decide",false,1);
		SceneManager.LoadScene (_sceneName);
		if(_sceneName != "TutorialScene")
		{
			SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
		}
		
	}
}