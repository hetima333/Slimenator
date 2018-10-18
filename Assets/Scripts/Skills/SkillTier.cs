using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tier List/Tier")]
public class SkillTier : ScriptableObject
{
    [SerializeField]
    private int
       _Multiplyer;

    public int GetMultiplyer()
    {
        return _Multiplyer;
    }
}
