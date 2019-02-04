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
    public OneRoomInfo[] _roomsInfos;

    // 作った部屋リスト
    public List<OneRoomInfo> _rooms;

    //マップ全体
    //private int[,] _map;
    public OneRoomInfo[,] _maps;

    [SerializeField]
    private GameObject _protal;

    //モデルサイズ
    static int modelSize = 150;
    public int ModelSize
    {
        get{ return modelSize; }
    }

    public void MapResize(int x = 0, int y = 0)
    {
        if(x == 0)
        {
            x = _width;
        }

        if(y == 0)
        {
            y = _depth;
        }

        //マップ全体の大きさ
        _maps = new OneRoomInfo[x, y];
    }

    /// <summary>
    /// 最初の部屋を生成する
    /// </summary>
    public void ChoiceFirstRoom()
    {
        //登録された部屋の種類を取得
        int count = _roomsInfos.Length;

        int firstRoom = 0;

        //部屋を生成する
        OneRoomInfo room = Instantiate(_roomsInfos[firstRoom], new Vector3(0,0,0), Quaternion.Euler(new Vector3(0, 180, 0)));
        room.transform.SetParent(transform);
        //地形がデータをマップに知らせる
        _maps[0, 0] = room;
        _rooms.Add(room);

        //部屋の通路の情報を取得
        return;

    }
    public List<OneRoomInfo> GetRooms()
    {
        return _rooms;
    }

    public OneRoomInfo[] GetRoomInfos()
    {
        return _roomsInfos;
    }

    /// <summary>
    /// 部屋のサイズ
    /// </summary>
    public Vector2Int GetSize()
    {
        return new Vector2Int(_width, _depth);
    }

    public void InitPortal()
    {
        OneRoomInfo info;
        var rand = Random.Range(1, _rooms.Count);
        print("rand=" + rand);
        info = _rooms[rand];
        Vector3 pos = info.gameObject.transform.position + new Vector3(10,0,0);

        _protal.transform.position = pos;   
    }

}
