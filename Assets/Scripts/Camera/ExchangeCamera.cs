using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ボス戦時の撮影方法スクリプトはオフ
        transform.GetChild(0).gameObject.GetComponent<MultipleTargetCamera>().enabled = false;

    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.O))
        {
            //撮影方法の切り替え
            ChangeShootingMethod();
        }
		
	}

    /// <summary>
    /// 撮影方法の切り替え
    /// </summary>
    public void ChangeShootingMethod()
    {
        //ボス戦時の撮影方法スクリプトはオン
        transform.GetChild(0).gameObject.GetComponent<MultipleTargetCamera>().enabled = true;
        //プレイヤー追尾スクリプトはオフ
        gameObject.GetComponent<CameraFollowTarget>().enabled = false;

    }
}
