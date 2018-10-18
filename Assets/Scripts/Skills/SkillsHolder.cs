using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHolder : SingletonMonoBehaviour<SkillsHolder>
{

    [SerializeField]
    private List<Skill>
        _CombinationSkillList = new List<Skill>();

    [SerializeField]
    private List<Skill>
        _BaseSkillList = new List<Skill>();

    public List<Skill>  GetSkillList()
    {
        List<Skill> temp = new List<Skill>();
        temp.AddRange(_BaseSkillList);
        temp.AddRange(_CombinationSkillList);

        return temp;
    }

    public List<Skill> GetBaseSkillList()
    {
        return _BaseSkillList;
    }

    public List<Skill> GetCombinationSkillList()
    {       
        return _CombinationSkillList;
    }
}
