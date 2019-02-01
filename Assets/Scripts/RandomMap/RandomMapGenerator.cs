using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    //横幅
    [SerializeField] private int _width = 5;
    //奥行き
    [SerializeField] private int _depth = 4;

    //部屋の登録
    public OneRoomInfo[] _rooms;

    //マップ全体
    //private int[,] _map;
    public OneRoomInfo[,] _maps;

    //モデルサイズ
    static int modelSize = 150;
    public int ModelSize
    {
        get{ return modelSize; }
    }

    void Awake()
    {
        //マップ全体の大きさ
        _maps = new OneRoomInfo[_width, _depth];

    }

    /// <summary>
    /// 最初の部屋を生成する
    /// </summary>
    public void ChoiceFirstRoom()
    {
        //登録された部屋の種類を取得
        int count = _rooms.Length;

        for (int z = 0; z < _depth; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                //System.Threading.Thread.Sleep(1);
                //60%の確率で生成する
                //if (RandomUtils.RandomJadge(0.6f))
                //{
                    int firstRoom = 0;
                    //do
                    //{
                    //    //最初に生成する部屋をランダムに決定する
                    //    firstRoom = RandomUtils.GetRandomInt(0, count - 1);
                    //    //通路が1つのみの部屋は除外
                    //} while ((_rooms[firstRoom].passagePos == (int)OneRoomInfo.PASSAGE.BACK) ||
                    //(_rooms[firstRoom].passagePos == (int)OneRoomInfo.PASSAGE.RIGHT) ||
                    //(_rooms[firstRoom].passagePos == (int)OneRoomInfo.PASSAGE.FRONT) ||
                    //(_rooms[firstRoom].passagePos == (int)OneRoomInfo.PASSAGE.LEFT));

                    //部屋を生成する
                    GameObject room = Instantiate(_rooms[firstRoom].gameObject, new Vector3(x * modelSize, 0, z * modelSize), Quaternion.Euler(new Vector3(0,180,0)));
                    room.transform.SetParent(transform);
                    //地形がデータをマップに知らせる
                    _maps[x, z] = _rooms[firstRoom];

                    //部屋の通路の情報を取得
                    //Debug.Log("map pos:" + new Vector2(x, z));
                    return;
                //}

            }
        }

    }

    public OneRoomInfo[] GetRoomInfos()
    {
        return _rooms;
    }


    /// <summary>
    /// 部屋のサイズ
    /// </summary>
    public Vector2Int GetSize()
    {
        return new Vector2Int(_width, _depth);
    }



}
