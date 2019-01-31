using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	[SerializeField]
	private Text _LangButtonText;
	// Use this for initialization
	void Start () {
		AudioManager.Instance.StopBGM();
		AudioManager.Instance.PlayBGM("TitleBGM",0.5f);
		_LangButtonText.text = Language.Instance.language;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
