using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeBase : MonoBehaviour {

    SlimeStats _stats;
    SlimeBaseState _state;
    Material _material;

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
    public void Init(float maxHealth, float velocity, SlimeStats.Slime_Type type)
    {
        _stats = new SlimeStats();
        _stats.MaxHealth = maxHealth;
        _stats.Health = _stats.MaxHealth;
        _stats.Velocity = velocity;
        _stats.Type = type;
        _stats.IsDead = false;
        _stats.MovementRange = Random.Range(5.0f, 10.0f);
        _stats.MaxMovementRange = 3.0f;
        _material = gameObject.GetComponent<Renderer>().material;

        // ENG: Initialize Slime material.
        // JAP: スライム材料を初期化する。
        switch (_stats.Type)
        {
            case SlimeStats.Slime_Type.SLIME_FIRE:
                Material.SetColor("_Color", Color.red);
                break;
            case SlimeStats.Slime_Type.SLIME_ICE:
                Material.SetColor("_Color", Color.blue);
                break;
            case SlimeStats.Slime_Type.SLIME_LIGHTING:
                Material.SetColor("_Color", Color.magenta);
                break;
            case SlimeStats.Slime_Type.SLIME_HEALTH:
                Material.SetColor("_Color", Color.green);
                break;
            case SlimeStats.Slime_Type.SLIME_GOLD:
                Material.SetColor("_Color", Color.yellow);
                break;
            default:
                break;
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
        transform.rotation = Quaternion.LookRotation(direction);
        transform.Translate(Vector3.forward * Time.deltaTime * _stats.Velocity);
    }

    // ENG: Returns the direction vector between slime and the destination.
    // JAP: スライムとデスティネーションの間の方向ベクトルを返します。
    private Vector3 GetDirection(Vector3 destination)
    {
        return (destination - transform.position).normalized;
    }
}
