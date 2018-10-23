using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : MonoBehaviour, IDamageable
{
    [SerializeField]
    private GameObject
        _SuckingParticle,
        _SuckingRadius;

    [SerializeField]
    private GameObject
        _PrefabInstance;

    private Stats
        _Player_Stats;

    private float
        _HP,
        _Money;

    private Queue<ElementType>
        _OrbSlot = new Queue<ElementType>();

    private List<Skill>
        _Skills = new List<Skill>();

    private Skill
        _CurrentSkillOutcome,
        _CurrentUseSkill;

    private int
        _CurrentSelection;

    private EnumHolder.States
        _Player_State;

    private Dictionary<EnumHolder.States, CheckFunctions> 
        _CheckFuntions = new Dictionary<EnumHolder.States, CheckFunctions>();

    private Animator
        _Animator;

	public float MaxHitPoint {get { return _Player_Stats.HealthProperties; } }
	public float HitPoint {get { return _HP; } }
    public float MoneyAmount { get { return _Money; } }
    [SerializeField]
    private GameObject
        _CastingPoint;

    private void Start()
    {
        _CurrentSkillOutcome = null;
        _CurrentSelection = 0;

        if (_Player_Stats != null)
            DestroyImmediate(_Player_Stats);

        _Player_Stats = EnumHolder.Instance.GetStats(gameObject.name);

        _HP = _Player_Stats.HealthProperties;
        _Money = 0;

        _Player_State = EnumHolder.States.IDLE;

        _Animator = gameObject.GetComponentInChildren<Animator>();

        _CheckFuntions.Add(EnumHolder.States.IDLE, IdleCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.MOVING, MovingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.KICKING, KickingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.CASTING, CastingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.DIE, DieCheckFunction);
    }

    private void IdleCheckFunction()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _Player_State = EnumHolder.States.MOVING;
            return;
        }
    }

    private void MovingCheckFunction()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            _Player_State = EnumHolder.States.IDLE;
            return;
        }
    }

    private void KickingCheckFunction()
    {

    }

    private void CastingCheckFunction()
    {
        _CurrentUseSkill.Engage(_CastingPoint, gameObject.transform.forward);

        if(_CurrentUseSkill.IsSkillOver())
        {
            _Player_State = EnumHolder.States.IDLE;
            DestroyImmediate(_CurrentUseSkill);
        }
    }

    private void DieCheckFunction()
    {
        if (!IsDead())
            _Player_State = EnumHolder.States.IDLE;
    }

    // Update is called once per frame
    private void Update()
    {
        _CheckFuntions[_Player_State]();
        _Animator.SetInteger("State", (int)_Player_State);

        if (_Player_State != EnumHolder.States.CASTING && 
            _Player_State != EnumHolder.States.KICKING && 
            _Player_State != EnumHolder.States.DIE)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!_SuckingParticle.activeSelf)
                    _SuckingParticle.SetActive(true);


                if (!_SuckingRadius.activeSelf)
                    _SuckingRadius.SetActive(true);

                _SuckingParticle.transform.Rotate(Vector3.up, 15);

            }
            else
            {
                if (_SuckingParticle.activeSelf)
                    _SuckingParticle.SetActive(false);

                if (_SuckingRadius.activeSelf)
                    _SuckingRadius.SetActive(false);
            }

            //Reset
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ResetOrbSlots();
            }

            //Storing of skill
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StoreSkills();
            }

            if (_Skills.Count > 0)
            {
                if (_Skills.Count > 1)
                {
                    float position = Input.GetAxis("Mouse ScrollWheel");
                    if (position > 0f)
                    {
                        --_CurrentSelection;                     
                    }
                    else if (position < 0f)
                    {
                        ++_CurrentSelection;
                    }
                }
                else
                    _CurrentSelection = 0;

                if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                    UseSkill();
                }
            }
        }
        else
        {
            if (_SuckingParticle.activeSelf)
                _SuckingParticle.SetActive(false);

            if (_SuckingRadius.activeSelf)
                _SuckingRadius.SetActive(false);
        }

        if (IsDead() && _Player_State != EnumHolder.States.DIE)
        {
            _Player_State = EnumHolder.States.DIE;
        }

        if (_CurrentSelection < 0)
            _CurrentSelection = _Skills.Count - 1;
        else if (_CurrentSelection > _Skills.Count - 1)
            _CurrentSelection = 0;
    }

    protected virtual void LateUpdate()
    {
        if (_Animator.speed != _Player_Stats.SpeedMultiplyerProperties)
            _Animator.speed = _Player_Stats.SpeedMultiplyerProperties;
    }

    public void StoreElementInOrb(ElementType type)
    {
        switch (type.name)
        {
            case "Gold":
                _Money += type.GetRandomAmount();
                break;

            case "Heal":
                _HP += type.GetRandomAmount();
                _HP = Mathf.Clamp(_HP, 0, _Player_Stats.HealthProperties);
                break;

            default:
                {

                    if (_CurrentSkillOutcome != null)
                    {
                        DestroyImmediate(_CurrentSkillOutcome);
                        _CurrentSkillOutcome = null;
                    }
                    
                    _OrbSlot.Enqueue(type);

                    if (_OrbSlot.Count > 3)
                        _OrbSlot.Dequeue();

                    bool HasUniqueCombination = true;

                    if (_OrbSlot.Count == 3)
                    {
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
                    }
                    else
                        HasUniqueCombination = false;

                    if (HasUniqueCombination)
                    {
                        foreach (Skill s in SkillsHolder.Instance.GetCombinationSkillList())
                        {
                            if (s.GetCombinationElements()[0] == _OrbSlot.ToArray()[0]
                                && s.GetCombinationElements()[1] == _OrbSlot.ToArray()[1]
                                && s.GetCombinationElements()[2] == _OrbSlot.ToArray()[2])
                            {
                                _CurrentSkillOutcome = Instantiate(s);
                                _CurrentSkillOutcome.name = s.name;
                                break;
                            }
                            else
                                _CurrentSkillOutcome = null;
                        }
                    }
                    else
                    {
                        foreach (Skill s in SkillsHolder.Instance.GetBaseSkillList())
                        {
                            if (s.GetBaseElement().Equals(_OrbSlot.ToArray()[0]))
                            {
                                int temp_tier = 0;

                                _CurrentSkillOutcome = Instantiate(s);
                                _CurrentSkillOutcome.name = s.name;

                                for(int i = 1; i < _OrbSlot.Count; ++i)
                                {
                                    if (s.GetBaseElement().Equals(_OrbSlot.ToArray()[i]))
                                        ++temp_tier;
                                    else
                                        break;
                                }

                                _CurrentSkillOutcome.SetSkillTier(EnumHolder.Instance._skillTier[temp_tier]);

                                break;
                            }
                            else
                                _CurrentSkillOutcome = null;
                        }
                    }
                }
                break;
        }      
    }

    public void StoreSkills()
    {
        if (_CurrentSkillOutcome != null)
        {
            _Skills.Add(_CurrentSkillOutcome);

            if (_Skills.Count > 3)
                _Skills.RemoveAt(0);

            ResetOrbSlots();

            _CurrentSkillOutcome = null;
        }
    }

    public void UseSkill()
    {
       if(_Skills.Count > 0)
        {
            _CurrentUseSkill = _Skills[_CurrentSelection];
            _CurrentUseSkill.Init();
            _Skills.RemoveAt(_CurrentSelection);
            _Player_State = EnumHolder.States.CASTING;
        }
    }

    public void ResetOrbSlots()
    {
        _OrbSlot.Clear();
        _CurrentSkillOutcome = null;
    }

    public EnumHolder.States GetPlayerState()
    {
        return _Player_State;
    }

    public Stats GetPlayerStats()
    {
        return _Player_Stats;
    }

    public bool IsDead()
    {
        return _HP <= 0;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 50), "Orb Slots");

        for (int i = 0; i < _OrbSlot.Count; ++i)
        {
            GUI.Box(new Rect(10, 50 * (i + 1), 100, 50), _OrbSlot.ToArray()[i].name);
        }

        GUI.Box(new Rect(10, 50 * 5, 500, 50), "Output: " + ((_CurrentSkillOutcome != null) ? ((_CurrentSkillOutcome.GetSkillTier() != null) ? _CurrentSkillOutcome.GetSkillTier().name + " " : "") + _CurrentSkillOutcome.name : "None"));

        GUI.Box(new Rect(1500, 10, 100, 50), "Skill Slots");
        for (int i = 0; i < _Skills.Count; ++i)
        {
            GUI.Box(new Rect(1500, 50 * (i + 1), 500, 50), ((_Skills.ToArray()[i].GetSkillTier() != null) ? _Skills.ToArray()[i].GetSkillTier().name + " " : "") + _Skills.ToArray()[i].name);
        }

        if (_Skills.Count > 0)
        {
            GUI.Box(new Rect(1500, 50 * 5, 500, 50), ((_Skills[_CurrentSelection].GetSkillTier() != null) ? _Skills[_CurrentSelection].GetSkillTier().name + " " : "") + _Skills[_CurrentSelection].name);
            GUI.Box(new Rect(1500, 50 * 6, 500, 50), "Description: " + _Skills[_CurrentSelection].GetDescription());
        }

        GUI.Box(new Rect(900, 10, 100, 50), "State: " + _Player_State);
    }

    public void TakeDamage(float Damage)
    {
        _HP -= Damage;
    }

    public Queue<ElementType> GetOrbsInSlot()
    {
        return _OrbSlot;
    }

    public List<Skill> GetSkillsInSlot()
    {
        return _Skills;
    }

    public int CurrentSelectedSkill()
    {
        return _CurrentSelection;
    }

    public Skill CurrentSkillOutcome()
    {
        return _CurrentSkillOutcome;
    }

    delegate void CheckFunctions();
}
