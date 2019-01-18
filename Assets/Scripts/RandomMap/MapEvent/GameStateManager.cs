using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //カメラ
    [SerializeField]
    private GameObject _clearCamera;
    [SerializeField]
    private GameObject _mainCamera;
    //演出用カメラ
    [SerializeField]
    private GameObject _cmCameras;

    // Use this for initialization
    void Start()
    {
        _enemyList = new List<GameObject>();
        _slimeList = new List<GameObject>();

        //通常BGM
        AudioManager.Instance.PlayBGM("Stage_bgm",2);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _warper.Warp();
            _exchangeCamera.ChangeShootingMethod();
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
        _norm--;
        if(_norm <= 0)
        {
            _isSearching = false;
            //BossBGM
            _warper.Warp();
            //撮影方法の切り替え
            _exchangeCamera.ChangeShootingMethod();
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
            //カメラの切り替え
            _clearCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);
            _cmCameras.transform.GetChild(0).gameObject.SetActive(false);
            _cmCameras.transform.GetChild(2).gameObject.SetActive(false);
            _cmCameras.transform.GetChild(1).gameObject.SetActive(true);
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
}
