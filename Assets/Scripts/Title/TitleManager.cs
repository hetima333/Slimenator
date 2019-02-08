using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	[SerializeField]
	private Text _LangButtonText;

	[SerializeField]
	private Text _DifficultyButtonText;

	[SerializeField]
	private Text _StartButtonText;

	[SerializeField]
	private Text _TutorialButtonText;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.StopBGM();
		AudioManager.Instance.PlayBGM("TitleBGM",0.5f);
		if(Language.Instance.language == "日本語")
			{
				if(Difficulty.Instance._difficulty == "NORMAL")
				{
					_DifficultyButtonText.text = "ノーマル";
				}
				else
				{
					_DifficultyButtonText.text = "ハード";
				}	
			}
			else{
				_DifficultyButtonText.text = Difficulty.Instance._difficulty;
			}
		_LangButtonText.text = Language.Instance.language;
		if(Language.Instance.language == "日本語")
		{
			_StartButtonText.text = "スタート";
			_TutorialButtonText.text = "チュートリアル";
		}
		else{
			_StartButtonText.text = "START";
			_TutorialButtonText.text = "TUTORIAL";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
