using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1つの部屋の情報
/// </summary>
/// 
//[System.Serializable]
public class OneRoomInfo : MonoBehaviour
{
    //1部屋の地形オブジェクト
    //public GameObject obj;
    //奥に通路があるか
    public bool _isBackPassage;
    //右に通路があるか
    public bool _isRightPassage;
    //手前に通路があるか
    public bool _isFrontPassage;
    //左に通路があるか
    public bool _isLeftPassage;

    //通路の位置
    public int passagePos;

    public bool _isSpawnerActive = true;

    public enum PASSAGE
    {
        BACK = 1,
        RIGHT = 2,
        FRONT = 4,
        LEFT = 8
    };

    void Start()
    {
        passagePos = 0;

        //通路の位置の判別
        if (_isBackPassage)
            passagePos |= (int)PASSAGE.BACK;
        if (_isRightPassage)
            passagePos |= (int)PASSAGE.RIGHT;
        if (_isFrontPassage)
            passagePos |= (int)PASSAGE.FRONT;
        if (_isLeftPassage)
            passagePos |= (int)PASSAGE.LEFT;

    }

    public void UpdateActived(bool isactive) 
    {
        var spawners = GetComponentsInChildren<SlimeSpawner>();
        foreach (var spawner in spawners)
        {
            spawner.UpdateActive(isactive);
        }
    }
}
