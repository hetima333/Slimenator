using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーや敵、スライムなどのオブジェクトをマップ上に挿入する
/// </summary>
public class InsertObject : MonoBehaviour {

    //生成するスライムスポナー
    private GameObject[] _createSlimeSpawners;

    //生成するスポナー
    private GameObject[] _spawnerPoint;

    //プレイヤー
    private GameObject _player;

    //スライムスポナー
    [SerializeField] private SetObject _slimeSpooner;

    //敵
    [SerializeField] private SetObject[] _enemys;

    //生成された敵
    public List<GameObject> _createEnemys;

    //生成するオブジェクトの種類
    private enum Type {
        //SLIME,
        ENEMY,
        NUM
        };

        //マップ情報
        private RandomMapGenerator _mapGen;

        // Use this for initialization
        void Start () {
        //プレイヤーの取得
        _player = GameObject.FindGameObjectWithTag ("Player");
        //マップ情報取得
        _mapGen = GetComponent<RandomMapGenerator> ();

        //プレイヤーの座標を設定
        SetPlayerPosition ();

        //全てのオブジェクトの生成
        CreateAllObject ();

    }

    /// <summary>
    /// プレイヤーの座標を設定する
    /// </summary>
    public void SetPlayerPosition () {
        //テスト：プレイヤー
        if (!_player)
            return;

        //最初に作られた部屋にプレイヤーのみ生成する
        var firstRoomPos = GetFirstRoom ();

        //プレイヤーの位置を設定
        _player.transform.position = new Vector3 (firstRoomPos.x * _mapGen.GetSize ().x, 2, firstRoomPos.y * _mapGen.GetSize ().y);

    }

    /// <summary>
    /// 最初に作られた部屋の取得
    /// </summary>
    /// <returns>幅と奥行きのvector2(int)</returns>
    public Vector2Int GetFirstRoom () {
        for (int z = 0; z < _mapGen.GetSize ().y; z++) {
            for (int x = 0; x < _mapGen.GetSize ().x; x++) {
                if (_mapGen._maps[x, z] != null) {
                    return new Vector2Int (x, z);
                }
            }
        }

        //部屋が無い場合
        Debug.Log ("Not room");
        return new Vector2Int (0, 0);
    }

    /// <summary>
    /// オブジェクトの生成
    /// </summary>
    //public void CreateObject()
    //{
    //    //生成するオブジェクトの取得
    //    _spawnerPoint = GameObject.FindGameObjectsWithTag("Spawner");

    //    foreach (var spawn in _spawnerPoint)
    //    {
    //        //60%の確率で
    //        if (RandomUtils.RandomJadge(0.6f))
    //        {
    //            //生成する物をランダムに決定する
    //            var type = RandomUtils.GetRandomInt((int)Type.SLIME, (int)Type.NUM - 1);

    //            switch (type)
    //            {
    //                //敵
    //                case (int)Type.ENEMY:
    //                    //敵のタイプをランダムに決定する
    //                    var enemyType = RandomUtils.GetRandomInt(0, _enemys.Length - 1);
    //                    //敵オブジェクトを生成する
    //                    GameObject enemyObj = ObjectManager.Instance.InstantiateWithObjectPooling(_enemys[enemyType]._object, spawn.transform.position, new Quaternion());
    //                    Enemy temp_enemy = enemyObj.GetComponent<Enemy>();
    //                    if (temp_enemy != null)
    //                    {
    //                        temp_enemy.Init(EnumHolder.Instance.GetStats(_enemys[enemyType]._object.name));
    //                    }
    //                    Debug.Log("create enemy");
    //                    break;
    //                //スライム
    //                case (int)Type.SLIME:
    //                    //スライムスポナーを生成する
    //                    GameObject slimeObj = ObjectManager.Instance.InstantiateWithObjectPooling(_slimeSpooner._object, spawn.transform.position, new Quaternion());
    //                    Debug.Log("create slime");
    //                    break;
    //                //その他
    //                default:
    //                    Debug.Log("Nothimg create object");
    //                    break;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 全てのオブジェクトを生成する
    /// </summary>
    public void CreateAllObject () {
        //スライムスポナーの生成
        CreateSlimeSpawner ();
        //敵の生成
        CreateEnemy ();

        GameStateManager.Instance._norm = (int)(_createEnemys.Count *0.8f);
        print (_createEnemys.Count);
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    public void CreateEnemy () {
        //生成するオブジェクトの取得
        _spawnerPoint = GameObject.FindGameObjectsWithTag ("Spawner");

        foreach (var spawn in _spawnerPoint) {
            //60%の確率で
            if (RandomUtils.RandomJadge (0.4f)) {
                //敵のタイプをランダムに決定する
                var enemyType = RandomUtils.GetRandomInt (0, _enemys.Length - 1);
                //敵オブジェクトを生成する
                GameObject enemyObj = ObjectManager.Instance.InstantiateWithObjectPooling (_enemys[enemyType]._object, spawn.transform.position, new Quaternion ());
                // 敵を部屋の子供にする
                enemyObj.transform.SetParent(spawn.transform.parent);
                //敵の数
                _createEnemys.Add (enemyObj);

                Enemy temp_enemy = enemyObj.GetComponent<Enemy> ();
                if (temp_enemy != null) {
                    temp_enemy.Init (EnumHolder.Instance.GetStats (_enemys[enemyType]._object.name));
                }
            }
        }
    }

    /// <summary>
    /// スライムスポナーの生成
    /// </summary>
    public void CreateSlimeSpawner () {
        //生成するスライムスポナーオブジェクトの取得
        _createSlimeSpawners = GameObject.FindGameObjectsWithTag ("SlimeSpawner");

        print (_createSlimeSpawners.Length);

        //スライムスポナーは必ず生成する
        foreach (var spawner in _createSlimeSpawners) {
            //生成する
            var spawner_obj = ObjectManager.Instance.InstantiateWithObjectPooling (_slimeSpooner._object, spawner.transform.position, new Quaternion ());

            // 敵を部屋の子供にする
            spawner_obj.transform.SetParent(spawner.transform.parent);
        }
    }

}