using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribute : MonoBehaviour
{
    //スライムオブジェクト
    [SerializeField]
    private AddObject[] _slimes;

    //敵オブジェクト
    [SerializeField]
    private AddObject[] _enemys;

    //マップ
    private int[,] _map;
    //幅
    private int _width;
    //奥行き
    private int _depth;
    //マップサイズ
    private int _mapSize;

    // Use this for initialization
    void Start()
    {

        //作られたマップの情報取得
        CreateRandomMap map = GetComponent<CreateRandomMap>();
        _map = map._map;
        _width = map._width;
        _depth = map._depth;
        _mapSize = map._mapSize;

        //スライム種類の数
        for (int i = 0; i < _slimes.Length; i++)
        {
            //最小値から最大値までのランダムの数のスライムを配置する
            CreateObject(_slimes[i]._object, RogueUtils.GetRandomInt(_slimes[i]._minGenerate, _slimes[i]._maxGenerate));
        }

        //敵種類の数
        for (int i = 0; i < _enemys.Length; i++)
        {
            //最小値から最大値までのランダムの数の敵を配置する
            CreateObject(_enemys[i]._object, RogueUtils.GetRandomInt(_enemys[i]._minGenerate, _enemys[i]._maxGenerate));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    ///// <summary>
    ///// スライムを配置する
    ///// </summary>
    ///// <param name="slime">スライムの種類</param>
    ///// <param name="num">配置する数</param>
    //private void CreateSlime(GameObject slime, int num)
    //{
    //    //スライムが設定されていない場合は設定の必要なし
    //    if (!slime)
    //    {
    //        Debug.Log("Slime is not set!!");
    //        return;
    //    }

    //    Position position;

    //    //配置する数になるまで
    //    for (int i = 0; i < num; i++)
    //    {
    //        do
    //        {
    //            //座標をランダムに決める
    //            var x = RogueUtils.GetRandomInt(0, _width - 1);
    //            var z = RogueUtils.GetRandomInt(0, _depth - 1);
    //            position = new Position(x, z);
    //        }
    //        //床があるところに限定する
    //        while (_map[position._x, position._z] != 1);

    //        //スライムを生成する
    //        Instantiate(slime, new Vector3(position._x * _mapSize, 0, position._z * _mapSize), new Quaternion());
    //        //GameObject slimes = Instantiate(slime, new Vector3(position._x, 0, position._z), new Quaternion());
    //        //slimes.transform.position = new Vector3(position._x, 0, position._z);
    //        //slimes.transform.SetParent(transform);
    //    }

    //}

    /// <summary>
    /// オブジェクトを配置する
    /// </summary>
    /// <param name="type">生成するオブジェクトの種類</param>
    /// <param name="num">配置する数</param>
    private void CreateObject(GameObject type, int num)
    {
        //オブジェクトが設定されていない場合は設定の必要なし
        if (!type)
        {
            Debug.Log("Slime is not set!!");
            return;
        }

        Position position;

        //配置する数になるまで
        for (int i = 0; i < num; i++)
        {
            do
            {
                //座標をランダムに決める
                var x = RogueUtils.GetRandomInt(0, _width - 1);
                var z = RogueUtils.GetRandomInt(0, _depth - 1);
                position = new Position(x, z);
            }
            //床があるところに限定する
            while (_map[position._x, position._z] != 1);

            //オブジェクトを生成する
            ObjectManager.Instance.InstantiateWithObjectPooling(type, new Vector3(position._x * _mapSize, 1, position._z * _mapSize), new Quaternion());

        }

    }

}
