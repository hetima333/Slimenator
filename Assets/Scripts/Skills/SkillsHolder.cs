using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHolder : SingletonMonoBehaviour<SkillsHolder>
{

    [SerializeField]
    private List<Skill>
        _SkillList = new List<Skill>();

	public List<Skill>  GetSkillList()
    {
        return _SkillList;
    }
}
