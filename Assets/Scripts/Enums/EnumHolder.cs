using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumHolder : SingletonMonoBehaviour<EnumHolder>
{
    // ENG: List of elements in the game.
    // JPN: ゲーム内の要素のリスト。

    [SerializeField]
    public SOList
    _Stat;

    public enum States
    {
        IDLE,
        MOVING,
        ATTACKING,
        CASTING,
        KICKING,
        DIE
    }

    public enum EffectType
    {
        SPEED,
        HEALTH,
    }

    public Stats GetStats(string InstanceName)
    {
        foreach(Stats s in _Stat.GetList())
        {
            if (s.GetPrefabName().Equals(InstanceName))
                return Instantiate(s);
        }

        return null;
    }
}
