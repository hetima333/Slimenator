using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;

public class ExchangeCamera : MonoBehaviour {

    private GameObject _player;

    private enum Camera
    {
        START,
        MAIN,
        BOSS_START,
        CLEAR
    }

    // Use this for initialization
    void Start () {
        //プレイヤーオブジェクトの取得
        _player = GameObject.FindGameObjectWithTag("Player");

        //ボス戦時の撮影方法スクリプトはオフ
        transform.GetChild((int)Camera.START).gameObject.GetComponent<MultipleTargetCamera>().enabled = false;
        //壁(ボス部屋)の透明化スクリプトはオフ
        gameObject.GetComponent<WallTransparent>().enabled = false;
        gameObject.GetComponent<ChangeWallMaterial>().enabled = false;

        //一時的にカメラを停止する
        StartCoroutine(Pause(2.5f));
    }

    // Update is called once per frame
    void Update () {

	}

    /// <summary>
    /// 撮影方法の切り替え
    /// </summary>
    public void ChangeShootingMethod()
    {
        //プレイヤー追尾スクリプトはオフ
        gameObject.GetComponent<CameraFollowTarget>().enabled = false;

        transform.GetChild((int)Camera.MAIN).gameObject.GetComponent<MultipleTargetCamera>().enabled = true;

        //壁(ボス部屋)の透明化スクリプトはオン
        gameObject.GetComponent<WallTransparent>().enabled = true;
        gameObject.GetComponent<ChangeWallMaterial>().enabled = true;

        //一時的にプレイヤーを停止する
        StartCoroutine(PlayerPause(3.0f));
    }

    /// <summary>
    /// 一定時間カメラをオフにする
    /// </summary>
    /// <param name="time"></param>
    public IEnumerator Pause(float time)
    {
        //メインカメラを消す
        IsActiveMainCamera(false);

        yield return new WaitForSeconds(time);

        //メインカメラをつける
        IsActiveMainCamera(true);
    }

    /// <summary>
    /// メインカメラをつけるかどうか
    /// </summary>
    public void IsActiveMainCamera(bool active)
    {
        //つける
        if (active)
        {
            //プレイ用(追尾)カメラに切り替える
            transform.GetChild((int)Camera.START).gameObject.SetActive(false);
            transform.GetChild((int)Camera.MAIN).gameObject.SetActive(true);

            //プレイヤーの移動スクリプト再生
            _player.GetComponent<ObservableUpdateTrigger>().enabled = true;
            _player.GetComponent<ObservableFixedUpdateTrigger>().enabled = true;
        }
        //消す
        else
        {
            //最初のカメラのみ表示
            transform.GetChild((int)Camera.START).gameObject.SetActive(true);
            transform.GetChild((int)Camera.MAIN).gameObject.SetActive(false);

            //プレイヤーの移動スクリプト停止
            _player.GetComponent<ObservableUpdateTrigger>().enabled = false;
            _player.GetComponent<ObservableFixedUpdateTrigger>().enabled = false;
        }
    }

    /// <summary>
    /// 一時的にプレイヤーを停止する
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator PlayerPause(float time)
    {
        _player.GetComponent<ObservableUpdateTrigger>().enabled = false;
        _player.GetComponent<ObservableFixedUpdateTrigger>().enabled = false;

        yield return new WaitForSeconds(time);

        _player.GetComponent<ObservableUpdateTrigger>().enabled = true;
        _player.GetComponent<ObservableFixedUpdateTrigger>().enabled = true;
    }

    public void StartProductSkip()
    {
        IsActiveMainCamera(false);
        IsActiveMainCamera(true);
    }
}
