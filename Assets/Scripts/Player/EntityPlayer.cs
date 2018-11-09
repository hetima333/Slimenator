﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : MonoBehaviour, IDamageable
{
    private enum DIRECTION
    {
        FRONT = 0,
        BACK,
        RIGHT,
        LEFT
    }

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
        _Money;

    private Status
        _Status;

    [SerializeField]
    private SOList
        _skillTier, 
        _baseSkill,
        _combiSkill;

    private Queue<ElementType>
        _OrbSlot = new Queue<ElementType>();

    private List<Skill>
        _Skills = new List<Skill>();

    private Skill
        _CurrentSkillOutcome,
        _CurrentUseSkill;

    private Vector3
        _PrevPosition;

    private int
        _CurrentSelection;

    private EnumHolder.States
        _Player_State;

    private DIRECTION
        _Player_Dir;

    private Dictionary<EnumHolder.States, CheckFunctions> 
        _CheckFuntions = new Dictionary<EnumHolder.States, CheckFunctions>();

    private Animator
        _Animator;

	public float MaxHitPoint {get { return _Player_Stats.MaxHealthProperties; } }
	public float HitPoint {get { return _Player_Stats.HealthProperties; } }
    public float MoneyAmount { get { return _Money; } }
    public float Speed
    {
        get
        {
            if (_Status != null)
            {
                return _Player_Stats.SpeedProperties *
                    ((100.0f - ((_Status.GetValue(EnumHolder.EffectType.SPEED) > 100) ? 100 :
                    ((_Status.GetValue(EnumHolder.EffectType.SPEED) < 0) ? 0 :
                    _Status.GetValue(EnumHolder.EffectType.SPEED)))) / 100.0f);
            }
            else
                return _Player_Stats.SpeedProperties;
        }
    }
    [SerializeField]
    private GameObject
        _CastingPoint;

    private void Awake()
    {
        _CurrentSkillOutcome = null;
        _CurrentSelection = 0;

        _Player_Stats = EnumHolder.Instance.GetStats(gameObject.name);

        _Player_Stats.HealthProperties = _Player_Stats.MaxHealthProperties;
        _Player_Stats.DamageMultiplyerProperties = _Player_Stats.HealthMultiplyerProperties = _Player_Stats.SpeedMultiplyerProperties = _Player_Stats.SuckingPowerMultiplyerProperties = 1;
        _Money = 0;

        _Player_State = EnumHolder.States.IDLE;
        _Player_Dir = DIRECTION.FRONT;

        _Animator = gameObject.GetComponentInChildren<Animator>();
        _Status = gameObject.GetComponent<Status>();
        _Status.Init();

        _CheckFuntions.Add(EnumHolder.States.IDLE, IdleCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.MOVING, MovingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.KICKING, KickingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.CASTING, CastingCheckFunction);
        _CheckFuntions.Add(EnumHolder.States.DIE, DieCheckFunction);
    }

    private void IdleCheckFunction()
    {
        _PrevPosition = gameObject.transform.position;

        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        if (InputManager.LS_Joystick() != Vector3.zero)
        {
            _Player_Dir = DIRECTION.FRONT;
            _Player_State = EnumHolder.States.MOVING;
            return;
        }
    }

    private void MovingCheckFunction()
    {
        //if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        if (InputManager.LS_Joystick() == Vector3.zero)
        {
            _Player_State = EnumHolder.States.IDLE;
            return;
        }

        float ForwardAngle = Vector3.Angle(gameObject.transform.forward.normalized, (_PrevPosition - gameObject.transform.position).normalized);
        float RightAngle = Vector3.Angle(gameObject.transform.right.normalized, (_PrevPosition - gameObject.transform.position).normalized);

        if (ForwardAngle < 30)
            _Player_Dir = DIRECTION.BACK;
        else if (ForwardAngle > 150)
            _Player_Dir = DIRECTION.FRONT;
        else if(ForwardAngle >= 30 && ForwardAngle <= 150)
        {
            if(RightAngle < 90)
                _Player_Dir = DIRECTION.RIGHT;
            else
                _Player_Dir = DIRECTION.LEFT;
        }

        _PrevPosition = gameObject.transform.position;
    }

    private void KickingCheckFunction()
    {

    }

    private void CastingCheckFunction()
    {
        _CurrentUseSkill.Engage(gameObject, _CastingPoint.transform.position, gameObject.transform.forward.normalized);

        if(_CurrentUseSkill.IsSkillOver() && _CurrentUseSkill.IsTimeOver() || Input.GetKey(KeyCode.Mouse1))
        {
            _CurrentUseSkill.Reset();
            _Player_State = EnumHolder.States.IDLE;
            Destroy(_CurrentUseSkill);
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
        TakeDamage(_Status.GetValue(EnumHolder.EffectType.HEALTH));

        _CheckFuntions[_Player_State]();
        _Animator.SetInteger("State", (int)_Player_State);
        _Animator.SetInteger("Direction", (int)_Player_Dir);

        if (_Player_State != EnumHolder.States.CASTING && 
            _Player_State != EnumHolder.States.KICKING && 
            _Player_State != EnumHolder.States.DIE &&
            Speed > 0)
        {
            if (InputManager.Suck_Input())
            {
                if (!_SuckingParticle.activeSelf)
                    _SuckingParticle.SetActive(true);


                if (!_SuckingRadius.activeSelf)
                    _SuckingRadius.SetActive(true);

                _SuckingParticle.transform.Rotate(Vector3.up, 15);

                _Animator.SetBool("IsSucking", true);
            }
            else
            {
                if (_SuckingParticle.activeSelf)
                    _SuckingParticle.SetActive(false);

                if (_SuckingRadius.activeSelf)
                    _SuckingRadius.SetActive(false);

                _Animator.SetBool("IsSucking", false);
            }

            //Reset
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ResetOrbSlots();
            }

            //Storing of skill
            if (InputManager.CombineOrbs_Input())
            {
                StoreSkills();
            }

            if (_Skills.Count > 0)
            {
                if (_Skills.Count > 1)
                {
                    float position = InputManager.SkillScroll_Input();
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

                if (InputManager.UseSkills_Input())
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
        if (_Animator.speed != (Speed / _Player_Stats.SpeedProperties) * _Player_Stats.SpeedMultiplyerProperties)
            _Animator.speed = (Speed / _Player_Stats.SpeedProperties) * _Player_Stats.SpeedMultiplyerProperties;
    }

    public void StoreElementInOrb(ElementType type)
    {
        switch (type.name)
        {
            case "Gold":
                _Money += type.GetRandomAmount();
                break;

            case "Heal":
                _Player_Stats.HealthProperties += type.GetRandomAmount();
                _Player_Stats.HealthProperties = Mathf.Clamp(_Player_Stats.HealthProperties, 0, _Player_Stats.MaxHealthProperties);
                break;

            default:
                {
                    if (_CurrentSkillOutcome != null)
                    {
                        Destroy(_CurrentSkillOutcome);
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
                        foreach (Skill s in _combiSkill.GetList())
                        {
                            if (s.GetCombinationElements()[0] == _OrbSlot.ToArray()[0]
                                && s.GetCombinationElements()[1] == _OrbSlot.ToArray()[1]
                                && s.GetCombinationElements()[2] == _OrbSlot.ToArray()[2])
                            {
                                _CurrentSkillOutcome = ScriptableObject.Instantiate(s);
                                _CurrentSkillOutcome.name = s.name;
                                break;
                            }
                            else
                                _CurrentSkillOutcome = null;
                        }
                    }
                    else
                    {
                        foreach (Skill s in _baseSkill.GetList())
                        {
                            if (s.GetBaseElement().Equals(_OrbSlot.ToArray()[0]))
                            {
                                int temp_tier = 0;

                                _CurrentSkillOutcome = ScriptableObject.Instantiate(s);
                                _CurrentSkillOutcome.name = s.name;

                                for(int i = 1; i < _OrbSlot.Count; ++i)
                                {
                                    if (s.GetBaseElement().Equals(_OrbSlot.ToArray()[i]))
                                        ++temp_tier;
                                    else
                                        break;
                                }

                                _CurrentSkillOutcome.SetSkillTier((SkillTier)_skillTier.GetList()[temp_tier]);
                                List<ElementType> temp = new List<ElementType>();
                                temp.AddRange(_OrbSlot.ToArray());
                                temp.RemoveAt(0);

                                if (temp.Count > 0)
                                    _CurrentSkillOutcome.SetElementType(temp);

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
        return _Player_Stats.HealthProperties <= 0;
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
        if (Damage > 0)
        {
            _Animator.SetTrigger("IsDamage");
            _Player_Stats.HealthProperties -= Damage;
        }
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

    public Status GetStatus()
    {
        return _Status;
    }

    delegate void CheckFunctions();
}
