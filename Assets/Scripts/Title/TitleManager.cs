using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	[SerializeField]
	private Text _LangButtonText;

	[SerializeField]
	private Text _DifficultyButtonText;

	// Use this for initialization
	void Start () {
		AudioManager.Instance.StopBGM();
		AudioManager.Instance.PlayBGM("TitleBGM",0.5f);
		_LangButtonText.text = Language.Instance.language;
		_DifficultyButtonText.text = Difficulty.Instance._difficulty;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
