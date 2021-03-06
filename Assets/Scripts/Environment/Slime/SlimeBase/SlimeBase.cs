﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SlimeBase : MonoBehaviour, ISuckable, IDamageable, IElement, IExplodable, IGrowable
{
    SlimeStats _stats;
    SlimeBaseState _state;
    Material _material;
    Animator _animator;
    Rigidbody _rigidbody;
    Stats _properties;
    Status _status;
    SkillTier _tier;

	public float MaxHitPoint { get { return _properties.MaxHealthProperties * _properties.HealthMultiplyerProperties; } }
	public float HitPoint { get { return _properties.HealthProperties; } }

	#region Getter/Setter
	public SlimeStats Stats
    {
        get
        {
            return _stats;
        }

        set
        {
            _stats = value;
        }
    }
    public Material Material
    {
        get
        {
            return _material;
        }

        set
        {
            _material = value;
        }
    }
    public Animator Animator
    {
        get
        {
            return _animator;
        }

        set
        {
            _animator = value;
        }
    }
    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }

        set
        {
            _rigidbody = value;
        }
    }

    #endregion

    protected virtual void Start()
    {
        SetState(new SlimeIdleState(this));

        _status = gameObject.GetComponent<Status>();
        _status.Init();

        _properties.HealthProperties = MaxHitPoint;
    }

    protected virtual void Update () {
        _state.Tick();
        _status.UpdateStatMultiplyer(ref _properties);
        TakeDamage(_status.GetValue(EnumHolder.EffectType.TAKEDAMAGE));

        if(gameObject.transform.localScale.x != _tier.GetMultiplyer())
        {
            gameObject.transform.localScale = new Vector3(_tier.GetMultiplyer(), _tier.GetMultiplyer(), _tier.GetMultiplyer());
        }

        //もし一定高度以下（落下したら）
        if(gameObject.transform.position.y <= -2)
        {
            //死ぬ
            Die();
        }
    }

    private void OnDisable()
    {
        if(transform.parent == null)
            return;

        var parent = transform.parent.parent;
        if(parent == null)
            return;

        if (parent.gameObject.activeSelf == false)
            ObjectManager.Instance.ReleaseObject(gameObject);
    }

    protected virtual void LateUpdate()
    {
        if (_animator.speed != _properties.SpeedMultiplyerProperties)
            _animator.speed = _properties.SpeedMultiplyerProperties;
    }
    public void TakeDamage(float dmg)
    {
        if (dmg >= 9999)
        {
            _properties.HealthProperties -= dmg;

            if (_properties.HealthProperties <= 0)
            {
                if (SceneManager.GetActiveScene().name == "TutorialScene") {
					// Tutorialシーンのみの動作 
					TutorialManager.Instance.SuckCount++;
				}
                Die();
            }
        }
    }

    // ENG: Every Slime will have a different results on death.
    // JAP: すべてのスライムは死の結果が異なります。
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    // ENG: Initialization 
    // JAP: 初期化。
    public virtual void Init(Stats newstats, float speedmultiplyer, ElementType type, SkillTier tier)
    {
        if (_rigidbody == null)
            CacheObject();

        if (_properties != null)
            Destroy(_properties);

        _properties = newstats;

        _stats = new SlimeStats();
        _properties.HealthProperties = MaxHitPoint;
        _properties.SpeedMultiplyerProperties = speedmultiplyer;
        _stats.Elementtype = type;
        _stats.IsDead = false;
        _stats.MovementRange = UnityEngine.Random.Range(5.0f, 10.0f);
        _stats.MaxMovementRange = 3.0f;
        _rigidbody.velocity = Vector3.zero;
        _tier = tier;

        Material.SetColor("_Color", _stats.Elementtype.GetColor());
    }

    public float Speed
    {
        get
        {
            return _properties.SpeedProperties * _properties.SpeedMultiplyerProperties;
        }
    }

    public void CacheObject()
    {
        _material = gameObject.GetComponentInChildren<Renderer>().material;
        _animator = gameObject.GetComponentInChildren<Animator>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // ENG: State Handler.
    // JAP: 状態ハンドラ.
    public void SetState(SlimeBaseState state)
    {
        if (_state != null)
            _state.OnStateExit();

        _state = state;

        if (_state != null)
            _state.OnStateEnter();
    }

    // ENG: Handles Movement and Rotation.
    // JAP: 移動と回転を処理します。
    public void MoveToward(Vector3 destination)
    {
        Vector3 direction = GetDirection(destination);
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (Speed * _properties.SpeedMultiplyerProperties) * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * (Speed * _properties.SpeedMultiplyerProperties));
    }

    // ENG: Returns the direction vector between slime and the destination.
    // JAP: スライムとデスティネーションの間の方向ベクトルを返します。
    private Vector3 GetDirection(Vector3 destination)
    {
        return (destination - transform.position).normalized;
    }

    protected virtual IEnumerator ISpawnTrail()
    {
        yield return new WaitForSeconds(0f); //add spawn delay if needed.
        GameObject trail = ObjectManager.Instance.InstantiateWithObjectPooling(GetElementType().GetEffect());
        trail.GetComponent<EnvironmentBase>().InitObjectWithLife(3.0f, GetPosition(), Vector3.one);
    }

    public virtual void SpawnTrail()
    {
        if (GetElementType().GetEffect() != null)
            StartCoroutine("ISpawnTrail");
    }


    public void SetTier(SkillTier newTier)
    {
        _tier = newTier;
    }


    public void Sacking()
    {
        return;
    }

    public ElementType GetElementType()
    {
        return _stats.Elementtype;
    }

    public SkillTier GetTier()
    {
        return _tier;
    }

    public void OnExplode()
    {
        //Instantiate(_ChannelingParticle, spawn_position, caster.transform.rotation, caster.transform);
    }

    public void Grow(SkillTier tier)
    {
        _tier = tier;
    }
}
