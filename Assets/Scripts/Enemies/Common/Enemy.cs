/// 敵の基本クラス
/// Base class of enemies
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必須コンポーネントの指定
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(EnemyMove))]


public class Enemy : MonoBehaviour, IDamageable
{

    //状態一覧(待機、自由移動、発見、戻り、攻撃、死亡)
    public enum State { IDLE,FREE, DISCOVERY, RETURN, ATTACK, DEAD }
    //今の状態
    [SerializeField]
    private State _currentState;
    public State CurrentState { get { return _currentState; } set { _currentState = value; } }
    //最大値
    [SerializeField]
    private float _maxHp;
    //体力
    [SerializeField]
    private float _hp;
    public float HP { get { return _hp; } set { _hp = value; } }
    //移動速度
    [SerializeField]
    private float _moveSpeed;
    public float Speed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    //索敵範囲
    public float _searchRange;
    //攻撃範囲
    public float _attackRange;
    //自由移動の幅
    public float _freeMoveRange;
    //所持金
    [SerializeField]
    private float _money;
    public float Money { get { return _money; } set { _money = value; } }
    //自由移動の目標座標
    public Vector3 _freeMovePosition;
    //初期座標
    public Vector3 _staetPosition;

    //行動中か？
    [SerializeField]
    private bool _isAction;
    public bool IsAction { get { return _isAction; } set { _isAction = value; } }
    //移動用リジットボディ
    private Rigidbody _rigidbody;
    public Rigidbody RigidbodyProperties { get { return _rigidbody; } set { _rigidbody = value; } }
    //索敵用コライダー
    private SphereCollider _sphereCol;
    public SphereCollider SphereColliderProperties { get { return _sphereCol; } set { _sphereCol = value; } }
    //狙う対象
    public GameObject _target;

    public SimpleAnimation _anim;

    //インタフェース用最大Hp取得
    public float MaxHitPoint{get{return _maxHp;}}
    //インタフェース用現在Hp取得
    public float HitPoint{get{return _hp;}}



    //ステータスのセット関数
    public void SetStatus(float maxHp,float speed,float searchRange,float attackRange,float moveRange,float money)
    {
        //初期はアイドル
        _currentState = State.IDLE;
        //最大体力
        _maxHp = maxHp;
        //体力
        _hp = _maxHp;
        //移動速度
        _moveSpeed = speed;
        //索敵範囲
        _searchRange = searchRange;
        //攻撃範囲
        _attackRange = attackRange;
        //自由移動の幅
        _freeMoveRange = moveRange;
        //所持金の設定
        _money = money;
        //初期位置の記憶
        _staetPosition = gameObject.transform.position;

        _anim = GetComponent<SimpleAnimation>();
    }


    //ダメージを受ける
    public void TakeDamage(float damage)
    {
        if (_currentState == State.DEAD) return;
        _hp -= damage;

        if (_hp <= 0)
        {
            _currentState = State.DEAD;
            StartCoroutine(Dying());
        }

    }


    //死亡コルーチン
    private IEnumerator Dying()
    {

        Debug.Log("Dead");
        _anim.CrossFade("Dead",0);

        yield return new WaitForSeconds(2);

        ObjectManager.Instance.ReleaseObject(gameObject);

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
             if(CurrentState != State.DEAD)
            {
            //Targetの設定
            _target = col.gameObject;
            //発見状態になる
            CurrentState = State.DISCOVERY;
            }
        }
    }

    //戦闘範囲離脱時の処理
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(CurrentState != State.DEAD)
            {
            //Targetの設定
            _target = null;
            //通常状態になる
            CurrentState = State.FREE;
            }
            
        }
    }


    //自分の本体に何かが接触した場合
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Skill")
        {
            //TODO take damage   
            TakeDamage(1);
        }
    }

}
