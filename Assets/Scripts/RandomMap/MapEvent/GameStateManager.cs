using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    public void DestroyMap()
    {
        // マップの破棄
        Destroy(_mapGenerator.gameObject);
    }
}
