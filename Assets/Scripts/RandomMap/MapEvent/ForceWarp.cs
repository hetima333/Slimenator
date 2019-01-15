using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWarp : MonoBehaviour
{

    public GameObject _target;
    public GameObject _Player;

    private GameObject _Boss;

    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private GameObject _bossStartCamera;
    [SerializeField]
    private GameObject _cmCameras;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Warp()
    {
        _Player.transform.position = _target.transform.position;
        _Boss = GameObject.Find("KingSlime(Clone)");
        //ボス開始演出カメラの切り替え
        _mainCamera.gameObject.SetActive(false);
        _bossStartCamera.gameObject.SetActive(true);
        _cmCameras.transform.GetChild(0).gameObject.SetActive(false);
        _cmCameras.transform.GetChild(1).gameObject.SetActive(false);
        _cmCameras.transform.GetChild(2).gameObject.SetActive(true);

        //演出終了後にカメラを切り替える
        Invoke("ChangeCamera", 3.0f);

        //ボスBGM再生
        AudioManager.Instance.PlayBGM("Boss_theme",1);
        _Boss.GetComponent<TestBoss>().WakeUp();
        GameStateManager.Instance.DestroyMap();
    }


    public void ChangeCamera()
    {
        _cmCameras.transform.GetChild(2).gameObject.SetActive(false);
        _bossStartCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);
    }

}

