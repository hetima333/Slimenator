using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWarp : MonoBehaviour
{

    public GameObject _target;
    public GameObject _Player;

    private bool _isActive = false;
    public bool IsActive
    {
        set { _isActive = value; }
    }
    private GameObject _Boss;
    private ExchangeCamera _camera;


    public ExchangeCamera ChangeCamera
    {
        set { _camera = value; }
    }

    public void Warp()
    {
        _Player.transform.position = _target.transform.position;
        _Boss = GameObject.Find("KingSlime(Clone)");
        //ボスBGM再生
        AudioManager.Instance.PlayBGM("Boss_theme",1);
        _Boss.GetComponent<TestBoss>().WakeUp();
        GameStateManager.Instance.DestroyMap();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isActive)
            return;

        if (collision.gameObject.tag == "Player")
        {
            // ワープ発動
            Warp();

            // 撮影カメラの切り替え
            _camera.ChangeShootingMethod();
        }
    }
}

