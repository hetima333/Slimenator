using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追加するオブジェクトの情報
/// </summary>
[System.Serializable]
public class AddObject
{
    //オブジェクト
    public GameObject _object;
    //生成する最小数
    public int _minGenerate;
    //生成する最大数
    public int _maxGenerate;

}
