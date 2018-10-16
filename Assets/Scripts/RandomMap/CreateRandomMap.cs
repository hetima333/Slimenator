using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomMap : MonoBehaviour {

    //幅
    [SerializeField]
    private int _width = 30;
    //奥行き
    [SerializeField]
    private int _depth = 20;
    //最大部屋数
    [SerializeField]
    private int _maxRoomNum = 6;

    //部屋のプレハブ
    [SerializeField]
    private GameObject _floorPrefab;
    //壁のプレハブ
    [SerializeField]
    private GameObject _wallPrefab;

    //マップ
    private int[,] _map;

    //マップサイズ
    [SerializeField]
    private int _mapSize = 1;


    // Use this for initialization
    void Start () {
        //マップサイズをスケールの基準に設定
        transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

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

        GenerateMap();


    }
	
	// Update is called once per frame
	void Update () {
        //マップサイズをスケールの基準に設定
        transform.localScale = new Vector3(_mapSize, _mapSize, _mapSize);

    }

    private void GenerateMap()
    {
        //マップを生成する
        _map = new MapGenerator().GenerateMap(_width, _depth, _maxRoomNum);

        //部屋のプレハブ
       // _floorPrefab = Resources.Load("Prefabs/RandomMap/Floor") as GameObject;
       // _wallPrefab = Resources.Load("Prefabs/RandomMap/Wall") as GameObject;

       // var floorList = new List<Vector3>();
       // var wallList = new List<Vector3>();

        //幅と奥行きの範囲内
        for (int z = 0; z < _depth; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_map[x, z] == 1)
                {
                    //部屋オブジェクトを生成する
                    Instantiate(_floorPrefab, new Vector3(x, 0, z), new Quaternion());
                    Debug.Log("Create floorPrefab map!!");
                }
                else
                {
                    Instantiate(_wallPrefab, new Vector3(x, 0, z), new Quaternion());
                    Debug.Log("Create wallPrefab map!!");
                }
            }
        }

    }

}
