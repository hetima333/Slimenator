using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeBase : MonoBehaviour, ISuckable, IDamageable, IElement
{
    SlimeStats _stats;
    SlimeBaseState _state;
    Material _material;
    Animator _animator;
    Rigidbody _rigidbody;
    [SerializeField]
    Transform _model;

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

    protected virtual void Start () {
        SetState(new SlimeIdleState(this));
    }

    protected virtual void Update () {
        _state.Tick();
    }
  
    public void TakeDamage(float dmg)
    {
        _stats.Health -= dmg;

        if (_stats.Health < 0)
        {
            Die();
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
    public virtual void Init(float maxHealth, float velocity, ElementType type)
    {
        if (_rigidbody == null)
            CacheObject();

        _stats = new SlimeStats();
        _stats.MaxHealth = maxHealth;
        _stats.Health = _stats.MaxHealth;
        _stats.Velocity = velocity;
        _stats.Elementtype = type;
        _stats.IsDead = false;
        _stats.MovementRange = UnityEngine.Random.Range(5.0f, 10.0f);
        _stats.MaxMovementRange = 3.0f;
        _rigidbody.velocity = Vector3.zero;
        _model.position = Vector3.zero;


        Material.SetColor("_Color", _stats.Elementtype.GetColor());
    }

    public void CacheObject()
    {
        _material = gameObject.GetComponentInChildren<Renderer>().material;
        _animator = gameObject.GetComponentInChildren<Animator>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        Transform[] tempArray = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in tempArray)
        {
            if (t.gameObject.GetInstanceID() != GetInstanceID())
            {
                _model = t;
            }
        }
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _stats.Velocity);
        transform.Translate(Vector3.forward * Time.deltaTime * _stats.Velocity);
    }

    // ENG: Returns the direction vector between slime and the destination.
    // JAP: スライムとデスティネーションの間の方向ベクトルを返します。
    private Vector3 GetDirection(Vector3 destination)
    {
        return (destination - transform.position).normalized;
    }

    public void Sacking()
    {
        return;
    }


    public ElementType GetElementType()
    {
        return _stats.Elementtype;
    }
}
