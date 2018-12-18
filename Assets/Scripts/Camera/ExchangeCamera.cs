using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ボス戦時の撮影方法スクリプトはオフ
        transform.GetChild(0).gameObject.GetComponent<MultipleTargetCamera>().enabled = false;
        //壁(ボス部屋)の透明化スクリプトはオフ
        gameObject.GetComponent<WallTransparent>().enabled = false;
        gameObject.GetComponent<ChangeWallMaterial>().enabled = false;

        //一時的にカメラを停止する
        StartCoroutine(Sleep(5.0f));
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
        //プレイヤー追尾スクリプトはオフ
        gameObject.GetComponent<CameraFollowTarget>().enabled = false;
        //ボス戦時の撮影方法スクリプトはオン
        transform.GetChild(0).gameObject.GetComponent<MultipleTargetCamera>().enabled = true;
        //壁(ボス部屋)の透明化スクリプトはオン
        gameObject.GetComponent<WallTransparent>().enabled = true;
        gameObject.GetComponent<ChangeWallMaterial>().enabled = true;
    }

    /// <summary>
    /// 一定時間カメラをオフにする
    /// </summary>
    /// <param name="time"></param>
    public IEnumerator Sleep(float time)
    {
        //gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log("OFF");
        yield return new WaitForSeconds(time);
        //gameObject.GetComponent<CameraFollowTarget>().enabled = true;
        //gameObject.SetActive(true);
        Debug.Log("ON");
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);


    }

}
