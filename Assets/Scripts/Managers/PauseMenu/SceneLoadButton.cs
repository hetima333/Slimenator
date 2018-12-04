using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MenuButtonBase {

	[SerializeField]
	private string _sceneName;

	protected override void OnExecute (PointerEventData e) {
		SceneManager.LoadScene (_sceneName);
		Pausable.Instance.Pausing = false;
	}
}