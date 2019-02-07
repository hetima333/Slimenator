using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageChangeButton : MenuButtonBase {

[SerializeField]
private Text _text;



	protected override void OnExecute (PointerEventData e) {
		if(Language.Instance.language == "日本語")
		{
			Language.Instance.language = "English";
			_text.text = Language.Instance.language;
		}
		else
		{
			Language.Instance.language = "日本語";
			_text.text = Language.Instance.language;
		}
		AudioManager.Instance.PlaySE("Decide");
	}
}
