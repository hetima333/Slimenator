using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MenuButtonBase {

	[SerializeField]
	private string _sceneName;

	protected override void OnExecute (PointerEventData e) {
		AudioManager.Instance.StopBGM();
		SceneManager.LoadScene (_sceneName);
		if(Pausable.Instance)
		{
			Pausable.Instance.Pausing = false;
		}
		
	}
}