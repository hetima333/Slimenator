using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyChangeButton : MenuButtonBase {

[SerializeField]
private Text _text;

	protected override void OnExecute (PointerEventData e) {
		if(Difficulty.Instance._difficulty == "NORMAL")
		{
			Difficulty.Instance._difficulty = "HARD";
			if(Language.Instance.language == "日本語")
			{
				_text.text = "ハード";
			}
			else{
				_text.text = Difficulty.Instance._difficulty;
			}
			
			Difficulty.Instance._statusMagnification = 1.5f;
		}
		else
		{
			Difficulty.Instance._difficulty = "NORMAL";
			if(Language.Instance.language == "日本語")
			{
				_text.text = "ノーマル";
			}
			else{
				_text.text = Difficulty.Instance._difficulty;
			}
			Difficulty.Instance._statusMagnification = 1.0f;
		}
		AudioManager.Instance.PlaySE("Decide");
	}
}