using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour {


[SerializeField]
private Text _text;

[SerializeField]
private string _str1;
[SerializeField]
private string _str2;
	// Use this for initialization
	void Start () {
		
		if(Language.Instance.language== "日本語")
		{
			_text.text = _str1;
		}
		else{
			_text.text = _str2;
		}	
	}
}
