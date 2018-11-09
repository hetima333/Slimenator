using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomMap : MonoBehaviour
{
    //幅
    [SerializeField]
    public int _width = 30;
    //奥行き
    [SerializeField]
    public int _depth = 20;
    //最大部屋数
    [SerializeField]
    private int _maxRoomNum = 6;
    //1つの部屋の範囲(大きさ)
    [SerializeField]
    private int _minRangeWidth = 6;

    //部屋のプレハブ
    //[SerializeField]
    //private GameObject _floorPrefab;
    //壁のプレハブ
    [SerializeField]
    private GameObject _wallPrefab;

    //マップ
    public int[,] _map;

    //マップ生成
    public MapGenerator _mapGenerator;

    //マップサイズ
    [SerializeField]
    public int _mapSize = 1;

    //プレイヤーの初期位置指定
    [SerializeField]
    [Header("-Initial position designation of player-")]
    private GameObject _player;

    //デバッグ用===============================================
    [Header("-Debug-")]
    //最初に作られた部屋の位置
    [SerializeField] private GameObject _roomStart;
    [SerializeField] private GameObject _roomWhidth;
    [SerializeField] private GameObject _roomDepth;
    [SerializeField] private GameObject _roomEnd;
    //==================================================ここまで

    // Use this for initialization
    void Start()
    {
        //マップサイズをスケールの基準に設定
        //transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

        ////幅と奥行きの範囲内に地形ベースを生成する
        //for(int x=0; x < _width; x++)
        //{
        //    for (int z = 0; z < _depth; z++)
        //    {
        //        //Cubeの生成
        //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        cube.transform.localPosition = new Vector3(x, 0, z);
        //        cube.transform.SetParent(transform);

        //    }
        //}

        //ランダムマップの作成
        GenerateMapObject();

        //プレイヤーの初期位置指定
        InitialPositionPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        //マップサイズをスケールの基準に設定
        transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

    }

    /// <summary>
    /// ランダムマップのオブジェクトを生成する
    /// </summary>
    private void GenerateMapObject()
    {
        //マップを生成する
        _mapGenerator = new MapGenerator();
        _map = _mapGenerator.GenerateMap(_width, _depth, _maxRoomNum, _minRangeWidth);

        //部屋のプレハブ
        // _floorPrefab = Resources.Load("Prefabs/RandomMap/Floor") as GameObject;
        // _wallPrefab = Resources.Load("Prefabs/RandomMap/Wall") as GameObject;

        // var floorList = new List<Vector3>();
        // var wallList = new List<Vector3>();

        ////床を一枚マップ全体の大きさに合わせて生成
        //GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //floor.transform.localScale = new Vector3(_width * _mapSize, _depth * _mapSize);

        //幅と奥行きの範囲内
        for (int z = 0; z < _depth; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_map[x, z] == (int)MapGenerator.MAP_STATUS.FLOOR)
                {
                    //Instantiate(_floorPrefab, new Vector3(x, 0, z), new Quaternion());
                    //部屋オブジェクトを生成する
                    //GameObject floor = Instantiate(_floorPrefab, new Vector3(x, 0, z), new Quaternion());
                    //floor.transform.SetParent(transform);
                    //Debug.Log("Create floorPrefab map!!");
                }
                else
                {
                    //Instantiate(_wallPrefab, new Vector3(x, 0, z), new Quaternion());
                    //壁オブジェクトを生成する
                    GameObject wall = Instantiate(_wallPrefab, new Vector3(x, 0, z), new Quaternion());
                    wall.transform.SetParent(transform);
                    //Debug.Log("Create wallPrefab map!!");
                }
            }
        }

    }

    /// <summary>
    /// プレイヤーの初期位置指定
    /// </summary>
    private void InitialPositionPlayer()
    {
        //プレイヤーが設定されていない場合は設定の必要なし
        if (!_player)
            return;

        //最初に作られた部屋の位置取得
        var startX = _mapGenerator.GetStartX(0);
        var endX = _mapGenerator.GetEndX(0);
        var startZ = _mapGenerator.GetStartZ(0);
        var endZ = _mapGenerator.GetEndZ(0);

        //デバッグ用=================================================================================================================
        //最後に生成された部屋
        var maxRoom = _mapGenerator.GetMaxRoom();
        _roomStart.transform.Translate(_mapGenerator.GetStartX(maxRoom - 1) * _mapSize, 1, _mapGenerator.GetStartZ(maxRoom - 1) * _mapSize);
        _roomWhidth.transform.Translate(_mapGenerator.GetEndX(maxRoom - 1) * _mapSize, 1, _mapGenerator.GetStartZ(maxRoom - 1) * _mapSize);
        _roomDepth.transform.Translate(_mapGenerator.GetStartX(maxRoom - 1) * _mapSize, 1, _mapGenerator.GetEndZ(maxRoom - 1) * _mapSize);
        _roomEnd.transform.Translate(_mapGenerator.GetEndX(maxRoom - 1) * _mapSize, 1, _mapGenerator.GetEndZ(maxRoom - 1) * _mapSize);

        //==================================================================================================================ここまで

        Position position;
        do
        {
            //座標をランダムに決める
            //var x = RogueUtils.GetRandomInt(0, _width - 1);
            //var z = RogueUtils.GetRandomInt(0, _depth - 1);
            var x = RogueUtils.GetRandomInt(startX, endX);
            var z = RogueUtils.GetRandomInt(startZ, endZ);
            position = new Position(x, z);
        }
        //床があるところに限定する
        while (_map[position._x, position._z] != (int)MapGenerator.MAP_STATUS.FLOOR);

        //プレイヤーの位置設定
        _player.transform.position = new Vector3(position._x * _mapSize, 0, position._z * _mapSize);
        //マップのプレイヤーの位置を追加
        _map[position._x, position._z] = (int)MapGenerator.MAP_STATUS.PLAYER;
        //プレイヤーから1マス目(隣)の範囲
        {
            //前後左右
            for (int i = 0, x = 0, z = -1; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //Debug.Log("x,y:" + new Vector2(x, z));
                //(0,1),(0,-1),(1,0),(-1,-0)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }
            //プレイヤーから見た斜め4方向
            for (int i = 0, x = -1, z = -1; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //(1,1),(1,-1),(-1,1),(-1,-1)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }
        }

        //プレイヤーから2個マス目の範囲
        {
            for (int i = 0, x = 0, z = -2; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //(0,-2),(2,0),(0,2),(-2,0)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }

            for (int i = 0, x = -2, z = -1; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //(-2,-1),(1,-2),(2,1),(-1,2)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }

            for (int i = 0, x = -2, z = -2; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //(-2,-2),(2,-2),(2,2),(-2,2)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }

            for (int i = 0, x = -1, z = -2; i < 4; x += z, z = x - z, x = z - x, i++)
            {
                //(-1,-2),(2,-1),(1,2),(-2,1)
                _map[position._x + x, position._z + z] = (int)MapGenerator.MAP_STATUS.PLAYER;
            }
        }

    }

}
