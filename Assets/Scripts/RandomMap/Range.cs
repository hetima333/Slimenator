using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 範囲
/// </summary>
public class Range {

    //最初
    public Position _start { get; set; }
    //最後
    public Position _end { get; set; }

    //幅を取得
    public int GetWidthX()
    {
        return _end._x - _start._x + 1;
    }

    //奥行きを取得
    public int GetWidthZ()
    {
        return _end._z - _start._z + 1;
    }

    public Range(Position start,Position end)
    {
        _start = start;
        _end = end;
    }

    public Range(int startX,int startZ,int endX,int endZ)
        :this(new Position(startX,startZ),new Position(endX, endZ)) { }

    public Range() : this(0, 0, 0, 0) { }

}
