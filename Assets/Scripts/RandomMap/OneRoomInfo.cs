using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1つの部屋の情報
/// </summary>
/// 
[System.Serializable]
public class OneRoomInfo
{
    //1部屋の地形オブジェクト
    public GameObject obj;
    //奥に通路があるか
    public bool isBackPassage;
    //右に通路があるか
    public bool isRightPassage;
    //手前に通路があるか
    public bool isFrontPassage;
    //左に通路があるか
    public bool isLeftPassage;

    //通路の位置
    public int passagePos = 0;

    public enum PASSAGE
    {
        BACK = 1,
        RIGHT = 2,
        FRONT = 4,
        LEFT = 8
    };

    /// <summary>
    /// 通路の位置の判別
    /// </summary>
    public void JudgmentPassage()
    {
        if (isBackPassage)
            passagePos |= (int)PASSAGE.BACK;
        if (isRightPassage)
            passagePos |= (int)PASSAGE.RIGHT;
        if (isFrontPassage)
            passagePos |= (int)PASSAGE.FRONT;
        if (isLeftPassage)
            passagePos |= (int)PASSAGE.LEFT;
    }

}
