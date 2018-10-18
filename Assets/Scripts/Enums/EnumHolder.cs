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

    public enum States
    {
        IDLE,
        MOVING,
        CASTING,
        KICKING,
        DIE
    }
}
