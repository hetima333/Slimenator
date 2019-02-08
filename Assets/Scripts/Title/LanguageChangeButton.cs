using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageChangeButton : MenuButtonBase {

[SerializeField]
private Text _text;

[SerializeField]
private Text _startText;

[SerializeField]
private Text _tutorialText;
[SerializeField]
private Text _difText;

	protected override void OnExecute (PointerEventData e) {
		if(Language.Instance.language == "日本語")
		{
			Language.Instance.language = "English";
			_text.text = Language.Instance.language;
			_startText.text = "START";
			_tutorialText.text = "TUTORIAL";
			_difText.text = Difficulty.Instance._difficulty;
			
			
			
		}
		else
		{
			Language.Instance.language = "日本語";
			_text.text = Language.Instance.language;
			_startText.text = "スタート";
			_tutorialText.text = "チュートリアル";

			if(Difficulty.Instance._difficulty == "NORMAL")
			{
				_difText.text = "ノーマル";
			}
			else{
				_difText.text = "ハード";
			}
		}

		AudioManager.Instance.PlaySE("Decide");
	}
}
