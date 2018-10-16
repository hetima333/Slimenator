﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {

    private int _mapSizeX; //マップサイズX軸
    private int _mapSizeZ; //マップサイズZ軸
    private int _maxRoomNum;    //最大部屋数

    private const int MINIMUM_RANGE_WIDTH = 6;

    //部屋(全体)
    private List<Range> _room = new List<Range>();
    //範囲(区画)
    private List<Range> _range = new List<Range>();
    //通路
    private List<Range> _passage = new List<Range>();
    //部屋から繋がっている通路
    private List<Range> _roomPassage = new List<Range>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// マップを生成する
    /// </summary>
    /// <param name="mapSizeX">マップサイズX軸</param>
    /// <param name="mapSizeZ">マップサイズZ軸</param>
    /// <param name="maxRoom">部屋の最大数</param>
    /// <returns></returns>
    public int[,] GenerateMap(int mapSizeX, int mapSizeZ, int maxRoom)
    {
        //マップサイズの設定
        _mapSizeX = mapSizeX;
        _mapSizeZ = mapSizeZ;
        _maxRoomNum = maxRoom;

        //マップ配列
        int[,] map = new int[mapSizeX, mapSizeZ];

        //部屋に分ける
        SeparateRoom(maxRoom);
        //分けた部屋の中に地面を生成する
        CreateGroundInTheRoom();

        //通路の表示
        foreach (Range passage in _passage)
        {
            for (int x = passage._start._x; x <= passage._end._x; x++)
            {
                for (int z = passage._start._z; z <= passage._end._z; z++)
                {
                    map[x, z] = 1;
                }
            }
            Debug.Log("passage!!!");
        }

        //部屋から繋がっている通路の表示
        foreach (Range roomPassage in _roomPassage)
        {
            for (int x = roomPassage._start._x; x <= roomPassage._end._x; x++)
            {
                for (int z = roomPassage._start._z; z <= roomPassage._end._z; z++)
                {
                    map[x, z] = 1;
                }
            }
            Debug.Log("roomPassage!!!");
        }

        //部屋の表示
        foreach (Range room in _room)
        {
            for (int x = room._start._x; x <= room._end._x; x++)
            {
                for (int z = room._start._z; z <= room._end._z; z++)
                {
                    map[x, z] = 1;
                }
            }
            Debug.Log("room!!!");
        }

        //通路の作成
        TrimPassage(ref map);

        return map;
    }

    /// <summary>
    /// 部屋に分ける
    /// </summary>
    /// <param name="maxRoom">部屋の最大数</param>
    public void SeparateRoom(int maxRoom)
    {
        _range.Add(new Range(0, 0, _mapSizeX - 1, _mapSizeZ - 1));

        //分けるかどうか
        bool isDevided;
        do
        {
            //縦から横に部屋に分ける
            isDevided = DevideRange(false);
            isDevided = DevideRange(true) || isDevided;

            //一つも分けなかったら終了
            if (_range.Count >= maxRoom)
                break;
        } while (isDevided);

        Debug.Log("SeparateRoom!!");

    }

    /// <summary>
    /// 除外範囲
    /// </summary>
    /// <param name="isVertical"></param>
    /// <returns></returns>
    public bool DevideRange(bool isVertical)
    {
        //除外するかどうか(除外しない)
        bool isDevided = false;

        //区画ごとに分けるかどうか
        List<Range> newRangeList = new List<Range>();
        foreach (Range range in _range)
        {
            //区画の数の制限を超えたらスキップ
            if (isVertical && range.GetWidthZ() < MINIMUM_RANGE_WIDTH * 2 + 1)
            {
                continue;
            }
            else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
            {
                continue;
            }

            System.Threading.Thread.Sleep(1);

            //40%の確立で分割しない(区画の数が1のときのみ必ず分割する)
            if (_range.Count > 1 && RogueUtils.RandomJadge(0.4f))
            {
                continue;
            }

            //長さ
            int length;
            //垂直なら
            if (isVertical)
                //奥行きの長さを取得
                length = range.GetWidthZ();
            else
                //幅の長さを取得(並行)
                length = range.GetWidthX();

            //余白(最少の区画サイズ2つ分引く)
            //右の最大幅から左の最少(0)に向かってみている
            int margin = length - MINIMUM_RANGE_WIDTH * 2;

            int baseIndex;
            if (isVertical)
                //配列のZ軸の始め取得
                baseIndex = range._start._z;
            else
                //配列のX軸の始め取得(並行)
                baseIndex = range._start._x;

            //除外(引いた分の残りをランダムで分割位置を決定する)
            int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + RogueUtils.GetRandomInt(1, margin) - 1;


            Range newRange = new Range();
            //垂直なら
            if(isVertical)
            {
                //横の範囲
                _passage.Add(new Range(range._start._x, devideIndex, range._end._x, devideIndex));
                //線を引く範囲を指定
                newRange = new Range(range._start._x, devideIndex + 1, range._end._x, range._end._z);
                range._end._z = devideIndex - 1;
            }
            else
            {
                //並行
                //縦の範囲
                _passage.Add(new Range(devideIndex, range._start._z, devideIndex, range._end._z));
                //線を引く範囲を指定
                newRange = new Range(devideIndex + 1, range._start._z, range._end._x, range._end._z);
                range._end._x = devideIndex - 1;
            }

            //追加リストに新しい区画を退避する
            newRangeList.Add(newRange);
            isDevided = true;
        }

        //追加リストに退避しておいた新しい区画を追加する
        _range.AddRange(newRangeList);

        Debug.Log("DevideRange!!");

        return isDevided;
    }

    /// <summary>
    /// 分けた部屋の中に地面を生成する
    /// </summary>
    public void CreateGroundInTheRoom()
    {
        //部屋をランダムにする
        _range.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

        //1区間毎に1部屋作る
        foreach(Range range in _range)
        {
            System.Threading.Thread.Sleep(1);

            //30%の確立で部屋を作らない
            //最大部屋数の半分に満たない場合は生成する
            if (_room.Count > _maxRoomNum / 2 && RogueUtils.RandomJadge(0.3f))
            {
                continue;
            }

            //区画の空き領域(余白)計算
            int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
            int marginZ = range.GetWidthZ() - MINIMUM_RANGE_WIDTH + 1;

            //軸をランダムに決める
            int randomX = RogueUtils.GetRandomInt(1, marginX);
            int randomZ = RogueUtils.GetRandomInt(1, marginZ);

            //軸の位置を算出
            int startX = range._start._x + randomX;
            int endX = range._end._x - RogueUtils.GetRandomInt(0, marginX - randomX) - 1;
            int startZ = range._start._z + randomZ;
            int endZ = range._end._z - RogueUtils.GetRandomInt(0, marginZ - randomZ) - 1;

            //部屋リストに追加
            Range room = new Range(startX, startZ, endX, endZ);
            _room.Add(room);

            //通路を作る
            CreatePassage(range,room);

            Debug.Log("CreateGroundInTheRoom!!");

        }

    }

    /// <summary>
    /// 通路を作成する
    /// </summary>
    /// <param name="range"></param>
    /// <param name="room"></param>
    private void CreatePassage(Range range,Range room)
    {
        List<int> direction = new List<int>();
        //範囲Xの最小値を0と設定(左)
        if (range._start._x != 0)
        {
            direction.Add(0);
        }
        //範囲Xの最大値を1と設定(右)
        if (range._end._x != _mapSizeX - 1)
        {
            direction.Add(1);
        }
        //範囲Zの最小値を2と設定(奥)
        if (range._start._z != 0)
        {
            direction.Add(2);
        }
        //範囲Zの最大値を3と設定(前)
        if (range._end._z != _mapSizeZ - 1)
        {
            direction.Add(3);
        }

        //通路リストをランダムに決める
        direction.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

        bool isFirst = true;
        foreach(int dir in direction)
        {
            System.Threading.Thread.Sleep(1);

            //80%の確立で通路を作らない
            if(!isFirst && RogueUtils.RandomJadge(0.8f))
            {
                continue;
            }
            else
            {
                //通路が無い場合は生成する
                isFirst = false;
            }

            int random;
            switch (dir)
            {
                case 0: //左
                    random = room._start._z + RogueUtils.GetRandomInt(1, room.GetWidthZ()) - 1;
                    _roomPassage.Add(new Range(range._start._x, random, room._start._x - 1, random));
                    break;
                case 1:　//右
                    random = room._start._z + RogueUtils.GetRandomInt(1, room.GetWidthZ()) - 1;
                    _roomPassage.Add(new Range(range._end._x + 1, random, room._end._x, random));
                    break;

                case 2: //奥
                    random = room._start._x + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
                    _roomPassage.Add(new Range(random, range._start._z, random, room._start._z- 1));
                    break;

                case 3: //前
                    random = room._start._x + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
                    _roomPassage.Add(new Range(random, range._end._z + 1, random, room._end._z));
                    break;
                default:
                    Debug.Log("dir Nothing!");
                    break;
            }
        }

    }

    /// <summary>
    /// 通路の作成
    /// </summary>
    /// <param name="map"></param>
    private void TrimPassage(ref int[,] map)
    {
        //繋がらなかった通路は削除する
        for (int i = _passage.Count - 1; i >= 0; i--)
        {
            Range passage = _passage[i];

            //垂直
            bool isVertical = passage.GetWidthZ() > 1;

            //通路が部屋から繋がっているかどうか
            bool isTrimTarget = true;
            if (isVertical)
            {
                int x = passage._start._x;
                for (int z = passage._start._z; z <= passage._end._z; z++)
                {
                    //隣に部屋があるとき
                    if (map[x - 1, z] == 1 || map[x + 1, z] == 1)
                    {
                        //削除しない
                        isTrimTarget = false;
                        break;
                    }
                }
            }
            else
            {
                int z = passage._start._z;
                for (int x = passage._start._x; x <= passage._end._x; x++)
                {
                    //隣に部屋があるとき
                    if (map[x, z - 1] == 1 || map[x, z + 1] == 1)
                    {
                        //削除しない
                        isTrimTarget = false;
                        break;
                    }
                }
            }

            //通路が部屋から繋がっていないものを削除する
            if (isTrimTarget)
            {
                _passage.Remove(passage);

                //マップ配列からも削除
                if (isVertical)
                {
                    int x = passage._start._x;
                    for (int z = passage._start._z; z <= passage._end._z; z++)
                    {
                        map[x, z] = 0;
                    }
                }
                else
                {
                    int z = passage._start._z;
                    for (int x = passage._start._x; x <= passage._end._x; x++)
                    {
                        map[x, z] = 0;
                    }
                }
            }

        }

        //外周に接している通路を別の通路との接続点まで削除する
        for (int x = 0; x < _mapSizeX - 1; x++)
        {
            //奥
            if (map[x, 0] == 1)
            {
                for (int z = 0; z < _mapSizeZ; z++)
                {
                    if (map[x - 1, z] == 1 || map[x + 1, z] == 1)
                        break;
                    //通路を塞ぐ
                    map[x, z] = 0;
                }
            }
            //前
            if (map[x, _mapSizeZ - 1] == 1)
            {
                for (int z = _mapSizeZ - 1; z >= 0; z--)
                {
                    if (map[x - 1, z] == 1 || map[x + 1, z] == 1)
                        break;
                    //通路を塞ぐ
                    map[x, z] = 0;
                }
            }
        }


        for (int z = 0; z < _mapSizeZ - 1; z++)
        {
            //右端
            if (map[0, z] == 1)
            {
                for (int x = 0; x < _mapSizeZ; x++)
                {
                    if (map[x, z - 1] == 1 || map[x, z + 1] == 1)
                        break;
                    //通路を塞ぐ
                    map[x, z] = 0;
                }
            }
            //左端
            if (map[_mapSizeX - 1, z] == 1)
            {
                for (int x = _mapSizeX - 1; x >= 0; x--)
                {
                    if (map[x, z - 1] == 1 || map[x, z + 1] == 1)
                        break;
                    //通路を塞ぐ
                    map[x, z] = 0;
                }
            }
        }
    }

}
