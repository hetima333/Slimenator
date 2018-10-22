using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumHolder : SingletonMonoBehaviour<EnumHolder>
{
    // ENG: List of elements in the game.
    // JPN: ゲーム内の要素のリスト。
    [SerializeField]
    public List<ElementType>
    _elements = new List<ElementType>();

    [SerializeField]
    public ElementType
    _None_Element;

    [SerializeField]
    public List<SkillTier>
    _skillTier = new List<SkillTier>();

    [SerializeField]
    public List<Stats>
    _Stat = new List<Stats>();

    public enum States
    {
        IDLE,
        MOVING,
        ATTACKING,
        CASTING,
        KICKING,
        DIE
    }

    public Stats GetStats(string InstanceName)
    {
        foreach(Stats s in _Stat)
        {
            if (s.GetPrefabName().Equals(InstanceName))
                return Instantiate(s);
        }

        return null;
    }
}
