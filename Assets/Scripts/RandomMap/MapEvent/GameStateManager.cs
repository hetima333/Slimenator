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

    private int _enemyNum;
    private bool _isSearching = false;
    //生成されたボスの数
    private int _bossNum;

    [SerializeField]
    private Canvas _canvas;

    // Use this for initialization
    void Start()
    {   
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

    public void IncreaseEnemy()
    {
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
            AudioManager.Instance.PlayBGM("Fan",1);
            SceneManager.UnloadSceneAsync("HUD");
            _canvas.gameObject.SetActive(true);
            

        }
    }

    public void DestroyMap()
    {
        // マップの破棄
        Destroy(_mapGenerator.gameObject);
    }
}
