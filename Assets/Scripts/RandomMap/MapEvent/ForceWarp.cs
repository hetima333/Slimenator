using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWarp : MonoBehaviour
{

    public GameObject _target;
    public GameObject _Player;

    private GameObject _Boss;

    [SerializeField]
    private GameObject _cameraHolder;
    private enum Camera
    {
        START,
        MAIN,
        BOSS_START,
        CLEAR
    }

    [SerializeField]
    private GameObject _cmCameras;
    private enum CMCamera
    {
        START,
        BOSS_START,
        CLEAR
    }

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
        _cameraHolder.transform.GetChild((int)Camera.MAIN).gameObject.SetActive(false);
        _cameraHolder.transform.GetChild((int)Camera.BOSS_START).gameObject.SetActive(true);

        _cmCameras.transform.GetChild((int)CMCamera.START).gameObject.SetActive(false);
        _cmCameras.transform.GetChild((int)CMCamera.CLEAR).gameObject.SetActive(false);
        _cmCameras.transform.GetChild((int)CMCamera.BOSS_START).gameObject.SetActive(true);

        //演出終了後にカメラを切り替える
        Invoke("ChangeCamera", 3.0f);

        //ボスBGM再生
        AudioManager.Instance.PlayBGM("Boss_theme",1);
        _Boss.GetComponent<TestBoss>().WakeUp();
        GameStateManager.Instance.DestroyMap();
    }


    public void ChangeCamera()
    {
        _cmCameras.transform.GetChild((int)CMCamera.BOSS_START).gameObject.SetActive(false);
        _cameraHolder.transform.GetChild((int)Camera.BOSS_START).gameObject.SetActive(false);
        _cameraHolder.transform.GetChild((int)Camera.MAIN).gameObject.SetActive(true);
    }

}

