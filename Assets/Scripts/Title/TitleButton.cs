using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleButton : MenuButtonBase {

	[SerializeField]
	private string _sceneName;

	protected override void OnExecute (PointerEventData e) {
		//決定SE
		AudioManager.Instance.PlaySE("Decide",false,1);

        FadeManager.Instance.StartTransition(1.0f, _sceneName);
		
	}
}