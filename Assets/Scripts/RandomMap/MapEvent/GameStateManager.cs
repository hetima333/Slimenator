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

    [SerializeField]
    private Canvas _clearCanvas;

    [SerializeField]
    private Canvas _gameOverCanvas;

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
        _enemyNum--;
        if(_enemyNum <= 0)
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
            AudioManager.Instance.PlayBGM("Fanfare",1);
            SceneManager.UnloadSceneAsync("HUD");
            _clearCanvas.gameObject.SetActive(true);
        }
    }

    public void DestroyMap()
    {
        print("DestroyMap");


        foreach (var enemy in _enemyList)
        {
            enemy.transform.SetParent(null);
            enemy.SetActive(false);
        }

        foreach (var slime in _slimeList)
        {
            slime.transform.SetParent(null);
            slime.GetComponent<SlimeBase>().Die();
        }

        // マップの破棄
        Destroy(_mapGenerator.gameObject);
    }


    public void PlayerDead()
    {
        //ゲームオーバー演出
        AudioManager.Instance.StopBGM();
        SceneManager.UnloadSceneAsync("HUD");
        _gameOverCanvas.gameObject.SetActive(true);
    }
}
