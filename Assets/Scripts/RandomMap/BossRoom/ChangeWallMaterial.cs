using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWallMaterial : MonoBehaviour {

    //現在半透明かどうか
    private bool _isTranslucent;

    private GameObject _wall;

    // Use this for initialization
    void Start () {
        //半透明にしない
        _isTranslucent = false;
        //壁タグがついているオブジェクトを取得する
        _wall = GameObject.FindGameObjectWithTag("Wall");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 半透明にするか
    /// </summary>
    /// <param name="isOn">半透明にするか</param>
    public void IsTranslucent(bool isOn)
    {
        if (isOn)
        {
            //既に半透明になっていれば変更する必要なし
            if (_isTranslucent) return;
            //壁を透明化する
            // _wall.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
            _isTranslucent = true;
        }
        else
        {
            //既に半透明にしないなら変更する必要なし
            if (!_isTranslucent) return;
            //半透明にしない
            //_wall.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _isTranslucent = false;
        }

    }
}
