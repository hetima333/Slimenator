using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx.Triggers;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{

    [SerializeField]
    private RandomMapGenerator _mapGenerator;
    public RandomMapGenerator Map
    {
        get { return _mapGenerator; }
    }

    [SerializeField]
    private ForceWarp _warper;

    [SerializeField]
    private ClearKeep _clearKeep;

    [SerializeField]
    private ExchangeCamera _exchangeCamera;

    private List<GameObject> _enemyList;

    private List<GameObject> _slimeList;

    private int _enemyNum;
    private bool _isSearching = false;
    //生成されたボスの数
    private int _bossNum;


    public int _norm = 0;

    [SerializeField]
    private Canvas _clearCanvas;

    [SerializeField]
    private Canvas _gameOverCanvas;

    private GameObject _player;

    private GameObject _warpPortal;

    //カメラ
    [SerializeField]
    private GameObject _cameraHolder;
    private enum Camera
    {
        START,
        MAIN,
        BOSS_START,
        CLEAR,
        CLEAR_KEEP
    }

    //演出用カメラ
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
        _enemyList = new List<GameObject>();
        _slimeList = new List<GameObject>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _warpPortal = GameObject.FindGameObjectWithTag("BossRoomWarpPortal");

        //通常BGM
        AudioManager.Instance.PlayBGM("Stage_bgm",2);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _warper.MyCamera = _exchangeCamera;
            _exchangeCamera.ChangeShootingMethod();
            _warper.Warp();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //スタート時のカメラ演出をスキップする
            _exchangeCamera.StartProductSkip();
        }
    }

    public void AddSlime(GameObject slime)
    {
        _slimeList.Add(slime);
    }

    public void IncreaseEnemy(GameObject enemy)
    {
        _enemyList.Add(enemy);
        _enemyNum++;
        _isSearching = true;
    }

    public void DecreaseEnemy()
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene") return;
        
        _norm--;
        if(_norm <= 0)
        {
            _isSearching = false;
            _warper.IsActive = true;

            //撮影カメラの切り替え
            _warper.MyCamera = _exchangeCamera;
        }
    }

    

    public void IncreaseBoss()
    {
        _bossNum++;
    }

    public void DecreaseBoss()
    {
        _bossNum--;
        if(_bossNum <= 0)
        {
            //CLEAR演出
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlaySE("Fanfare");
            SceneManager.UnloadSceneAsync("HUD");
            _clearCanvas.gameObject.SetActive(true);
            ClearChangeCamera();

            //カメラをキープする
            StartCoroutine(KeepCamera(4.0f));
        }
    }

    public void DestroyMap()
    {
        print("DestroyMap");

        GameObject trashObjPool = new GameObject();
        trashObjPool.name = "Trash";

        foreach (var enemy in _enemyList)
        {
            enemy.transform.SetParent(trashObjPool.transform);
            enemy.SetActive(false);
        }

        foreach (var slime in _slimeList)
        {
            slime.transform.SetParent(trashObjPool.transform);
            slime.GetComponent<SlimeBase>().Die();
        }

        // マップの破棄
        _mapGenerator.gameObject.SetActive(false);
    }


    public void PlayerDead()
    {
        //ゲームオーバー演出
        AudioManager.Instance.StopBGM();
        SceneManager.UnloadSceneAsync("HUD");
        _gameOverCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// カメラをクリアー演出用カメラに切り替える
    /// </summary>
    public void ClearChangeCamera()
    {
        //プレイヤーの移動を停止
        PlayerMoveStop();

        _cameraHolder.transform.GetChild((int)Camera.CLEAR).gameObject.SetActive(true);
        _cameraHolder.transform.GetChild((int)Camera.MAIN).gameObject.SetActive(false);

        _cmCameras.transform.GetChild((int)CMCamera.START).gameObject.SetActive(false);
        _cmCameras.transform.GetChild((int)CMCamera.BOSS_START).gameObject.SetActive(false);
        _cmCameras.transform.GetChild((int)CMCamera.CLEAR).gameObject.SetActive(true);
    }

    /// <summary>
    /// プレイヤーの入力移動を一時停止する
    /// </summary>
    public void PlayerMoveStop()
    {
        _player.GetComponent<ObservableUpdateTrigger>().enabled = false;
        _player.GetComponent<ObservableFixedUpdateTrigger>().enabled = false;
        //プレイヤーの向きを正面に指定
        _player.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        //プレイヤーをワープ位置に指定
        _player.gameObject.transform.position = _warpPortal.transform.position;
    }

    public IEnumerator KeepCamera(float time)
    {
        yield return new WaitForSeconds(time);

        _cameraHolder.transform.GetChild((int)Camera.CLEAR).gameObject.SetActive(false);
        _cameraHolder.transform.GetChild((int)Camera.CLEAR_KEEP).gameObject.SetActive(true);
        _clearKeep.CameraPosition();
    }

}
