using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{

    [SerializeField]
    private InsertObject _enemyCreater;

    [SerializeField]
    private ForceWarp _warper;
    [SerializeField]
    private ExchangeCamera _exchangeCamera;

    private int _enemyNum;
    private bool _isSearching = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_enemyNum);
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
            _warper.Warp();
            //撮影方法の切り替え
            _exchangeCamera.ChangeShootingMethod();

        }
    }

    public void DestroyMap()
    {
        // マップの破棄
        Destroy(_enemyCreater.gameObject);
    }
}
