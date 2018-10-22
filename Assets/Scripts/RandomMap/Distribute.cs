using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribute : MonoBehaviour {

    //スライムオブジェクト
    [SerializeField]
    private AddObject[] _slimes;

    ////スライムオブジェクト
    //[SerializeField]
    //private GameObject[] _slimes;
    //青のスライムオブジェクト
    //[SerializeField]
    //private GameObject _blueSlime;
    ////黄のスライムオブジェクト
    //[SerializeField]
    //private GameObject _yellowSlime;

    ////配置する赤のスライムの数
    //[SerializeField]
    //private int _redSlimeNum;
    ////配置する青のスライムの数
    //[SerializeField]
    //private int _blueSlimeNum;
    ////配置する黄のスライムの数
    //[SerializeField]
    //private int _yellowSlimeNum;

    //マップ
    private int[,] _map;
    //幅
    private int _width;
    //奥行き
    private int _depth;

	// Use this for initialization
	void Start () {

        //作られたマップの情報取得
        CreateRandomMap map = GetComponent<CreateRandomMap>();
        _map = map._map;
        _width = map._width;
        _depth = map._depth;

        //スライム種類の数
        for (int i = 0; i < _slimes.Length; i++)
        {
            //最小値から最大値までのランダムの数のスライムを配置する
            CreateSlime(_slimes[i]._object, RogueUtils.GetRandomInt(_slimes[i]._minGenerate, _slimes[i]._maxGenerate));
        }

        ////赤のスライムを配置する
        //CreateSlime(_redSlime, _redSlimeNum);
        ////青のスライムを配置する
        //CreateSlime(_blueSlime, _blueSlimeNum);
        ////黄のスライムを配置する
        //CreateSlime(_yellowSlime, _yellowSlimeNum);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// スライムを配置する
    /// </summary>
    /// <param name="slime">スライムの種類</param>
    /// <param name="num">配置する数</param>
    private void CreateSlime(GameObject slime, int num)
    {
        //スライムが設定されていない場合は設定の必要なし
        if (!slime)
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

            //スライムを生成する
            Instantiate(slime, new Vector3(position._x, 0, position._z), new Quaternion());
            //GameObject slimes = Instantiate(slime, new Vector3(position._x, 0, position._z), new Quaternion());
            //slimes.transform.position = new Vector3(position._x, 0, position._z);
            //slimes.transform.SetParent(transform);
        }

    }

}
