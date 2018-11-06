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
    //マップサイズ
    private int _mapSize;

    // Use this for initialization
    void Start()
    {
        //作られたマップの情報取得
        CreateRandomMap map = GetComponent<CreateRandomMap>();
        _map = map._map;
        _mapSize = map._mapSize;

        //スライム種類の数
        for (int i = 0; i < _slimes.Length; i++)
        {
            //最小値から最大値までのランダムの数のスライムを配置する
            CreateObject(_slimes[i]._object, RogueUtils.GetRandomInt(_slimes[i]._minGenerate, _slimes[i]._maxGenerate), MapGenerator.MAP_STATUS.SLIME);
        }

        //敵種類の数
        for (int i = 0; i < _enemys.Length; i++)
        {
            //最小値から最大値までのランダムの数の敵を配置する
            CreateObject(_enemys[i]._object, RogueUtils.GetRandomInt(_enemys[i]._minGenerate, _enemys[i]._maxGenerate), MapGenerator.MAP_STATUS.ENEMY);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // /// <summary>
    // /// オブジェクトを配置する
    // /// </summary>
    // /// <param name="type">生成するオブジェクトの種類</param>
    // /// <param name="num">配置する数</param>
    // private void CreateObject(GameObject type, int num, MapGenerator.MAP_STATUS status)
    // {
    //     //オブジェクトが設定されていない場合は設定の必要なし
    //     if (!type)
    //     {
    //         Debug.Log("Object is not set!!");
    //         return;
    //     }

    //     MapGenerator generator = GetComponent<CreateRandomMap>()._mapGenerator;
    //     Position position;

    //     //配置する数になるまで
    //     for (int i = 0; i < num; i++)
    //     {
    //         do
    //         {
    //             //部屋番号をランダムに決定(プレイヤーがいる部屋(0)とボス部屋(最後)には生成しない)
    //             var roomNum = RogueUtils.GetRandomInt(1, generator.GetMaxRoom() - 2);

    //             //最初に作られた部屋の位置取得
    //             var startX = generator.GetStartX(roomNum);
    //             var endX = generator.GetEndX(roomNum);
    //             var startZ = generator.GetStartZ(roomNum);
    //             var endZ = generator.GetEndZ(roomNum);

    //             //座標をランダムに決める
    //             var x = RogueUtils.GetRandomInt(startX, endX);
    //             var z = RogueUtils.GetRandomInt(startZ, endZ);
    //             position = new Position(x, z);
    //         }
    //         //床があるところに限定し、自分以外のオブジェクトと重ならないようにする
    //         while ((_map[position._x, position._z] != (int)MapGenerator.MAP_STATUS.FLOOR));

    //         //オブジェクトを生成する
    //         ObjectManager.Instance.InstantiateWithObjectPooling(type, new Vector3(position._x * _mapSize, 1, position._z * _mapSize), new Quaternion());
    //         //マップに情報を登録する
    //         _map[position._x, position._z] = (int)status;
    //     }

    // }

    /// <summary>
    /// オブジェクトを配置する
    /// </summary>
    /// <param name="type">生成するオブジェクトの種類</param>
    /// <param name="num">配置する数</param>
    private void CreateObject(GameObject type, int num, MapGenerator.MAP_STATUS status)
    {
        //オブジェクトが設定されていない場合は設定の必要なし
        if (!type)
        {
            Debug.Log("Object is not set!!");
            return;
        }

        MapGenerator generator = GetComponent<CreateRandomMap>()._mapGenerator;
        Position position = new Position();

        //部屋の番号順にランダムにオブジェクトを配置する(プレイヤーがいる部屋(0)とボス部屋(最後)には生成しない)
        for (int roomNum = 1; roomNum < generator.GetMaxRoom() - 1; roomNum++)
        {
            //配置する数になるまで
            for (int i = 0; i < num; i++)
            {
                do
                {
                    //最初に作られた部屋の位置取得
                    var startX = generator.GetStartX(roomNum);
                    var endX = generator.GetEndX(roomNum);
                    var startZ = generator.GetStartZ(roomNum);
                    var endZ = generator.GetEndZ(roomNum);

                    //座標をランダムに決める
                    var x = RogueUtils.GetRandomInt(startX, endX);
                    var z = RogueUtils.GetRandomInt(startZ, endZ);
                    position = new Position(x, z);
                }
                //床があるところに限定し、自分以外のオブジェクトと重ならないようにする
                while (_map[position._x, position._z] != (int)MapGenerator.MAP_STATUS.FLOOR);

                //オブジェクトを生成する
                ObjectManager.Instance.InstantiateWithObjectPooling(type, new Vector3(position._x * _mapSize, 1, position._z * _mapSize), new Quaternion());
                //マップに情報を登録する
                _map[position._x, position._z] = (int)status;

            }

        }

    }

}
