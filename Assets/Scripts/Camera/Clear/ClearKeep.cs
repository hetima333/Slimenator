using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearKeep : MonoBehaviour {

    [SerializeField]
    private Camera _clearCamera;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// カメラの座標を設定する
    /// </summary>
    public void CameraPosition()
    {
        //クリアー演出カメラの位置を設定
        var pos = _clearCamera.gameObject.transform.position;
        gameObject.transform.position = pos;
        var rot = _clearCamera.gameObject.transform.rotation;
        gameObject.transform.rotation = rot;
    }
}
