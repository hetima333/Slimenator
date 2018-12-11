using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{

    [SerializeField]
    private InsertObject _enemyCreater;

    [SerializeField]
    private ForceWarp _warper;
    private int _enemyNum;
    private bool _isSearching = false;
    //生成されたボスの数
    private int _bossNum;

    // Use this for initialization
    void Start()
    {   
        //通常BGM
        AudioManager.Instance.PlayBGM("Stage_bgm",1);
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
        }
    }

    public void DestroyMap()
    {
        // マップの破棄
        Destroy(_enemyCreater.gameObject);
    }
}
