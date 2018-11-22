using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランダムユーティリティ
/// </summary>
public class RandomUtils
{

    /// <summary>
    /// 最小値から最大値までの間にある値を無作為に抽出する
    /// </summary>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns></returns>
    public static int GetRandomInt(int min, int max)
    {
        return min + Mathf.FloorToInt(Random.value * (max - min + 1));
    }

    /// <summary>
    /// 確率
    /// </summary>
    /// <param name="rate">確率値(最大1.0f)</param>
    /// <returns></returns>
    public static bool RandomJadge(float rate)
    {
        return Random.value < rate;
    }

}
