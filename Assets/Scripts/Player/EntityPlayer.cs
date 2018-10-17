using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject
        _SuckingParticle,
        _SuckingRadius;

    [SerializeField]
    private PlayerStats
        _Player_Stats;

    private Queue<ElementType>
        _OrbSlot = new Queue<ElementType>();

    private List<Skill>
        _Skills = new List<Skill>();

    private Skill
        _CurrentSelectedSkill;

    private void Start()
    {
        _CurrentSelectedSkill = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!_SuckingParticle.activeSelf)
                _SuckingParticle.SetActive(true);

            if (!_SuckingRadius.activeSelf)
                _SuckingRadius.SetActive(true);

        }
        else
        {
            if (_SuckingParticle.activeSelf)
                _SuckingParticle.SetActive(false);

            if (_SuckingRadius.activeSelf)
                _SuckingRadius.SetActive(false);
        }


        for(int i = 0; i < _OrbSlot.Count; ++i)
        {
            Debug.Log("Orb " + (i + 1) + ": " + _OrbSlot.ToArray()[i].name);
        }

        Debug.Log("Currently Using Skill: " + ((_CurrentSelectedSkill != null) ? _CurrentSelectedSkill.name : "None"));

        for (int i = 0; i < _Skills.Count; ++i)
        {
            Debug.Log("Skill " + (i + 1) + ": " + _Skills.ToArray()[i].name);
        }

        for (int i = _OrbSlot.Count; i < 3; ++i)
        {
           // _OrbSlot.Enqueue(Slime.)
        }
    }

    public void StoreElementInOrb(ElementType type)
    {
        _OrbSlot.Enqueue(type);

        if (_OrbSlot.Count > 3)
            _OrbSlot.Dequeue();

        bool HasUniqueCombination = true;

        for (int i = 0; i < _OrbSlot.Count; ++i)
        {
            for (int j = i + 1; j < _OrbSlot.Count; ++j)
            {
                if (_OrbSlot.ToArray()[i].Equals(_OrbSlot.ToArray()[j]))
                {
                    HasUniqueCombination = false;
                    break;
                }
            }
        }

        foreach (Skill s in SkillsHolder.Instance.GetSkillList())
        {
            if (HasUniqueCombination)
            {
                if (s.GetCombinationElements().Equals(_OrbSlot.ToArray()))
                {
                    _CurrentSelectedSkill = s;
                    Debug.Log("Found Skill");
                    break;
                }
            }
            else
            {
                if (s.GetBaseElement().Equals(_OrbSlot.ToArray()[0]))
                {
                    _CurrentSelectedSkill = s;
                    Debug.Log("Found Skill");
                    break;
                }
            }
        }
    }

    public void StoreSkills()
    {
        if (_CurrentSelectedSkill != null)
        {
            _Skills.Add(_CurrentSelectedSkill);

            if (_Skills.Count > 3)
                _Skills.RemoveAt(0);

            _CurrentSelectedSkill = null;
        }
    }
}
