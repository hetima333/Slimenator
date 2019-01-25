using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleButton : MenuButtonBase {

	[SerializeField]
	private string _sceneName;

	[SerializeField]
	private string _HUD;

	protected override void OnExecute (PointerEventData e) {
		//決定SE
		AudioManager.Instance.PlaySE("Decide",false,1);
		SceneManager.LoadScene (_sceneName);
		Application.LoadLevelAdditive (_HUD);
	}
}