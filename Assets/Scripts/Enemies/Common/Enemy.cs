/// 敵の基本クラス
/// Base class of enemies
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/25
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必須コンポーネントの指定
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (EnemyMove))]
[RequireComponent (typeof (SimpleAnimation))]

public class Enemy : MonoBehaviour, IDamageable {

    //種類
    public enum Type { MEEL, RANGE, TANK, BOSS }
    private Type _enemyType;
    public Type EnemyType { get { return _enemyType; } set { _enemyType = value; } }

    //状態一覧(待機、自由移動、発見、戻り、攻撃、死亡)
    public enum State { IDLE, FREE, DISCOVERY, RETURN, ATTACK, DEAD }

    //今の状態
    [SerializeField]
    private State _currentState;
    public State CurrentState { get { return _currentState; } set { _currentState = value; } }
    //状態異常
    public enum BadState { NONE, FIRE, FREEZ, PARALYSIS }
    private BadState _badState;
    public BadState CurrentBadState { get { return _badState; } set { _badState = value; } }

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
    public Vector3 _startPosition;

    //行動中か？
    [SerializeField]
    private bool _isAction;
    public bool IsAction { get { return _isAction; } set { _isAction = value; } }
    //移動用リジットボディ
    private Rigidbody _rigidbody;
    public Rigidbody RigidbodyProperties { get { return _rigidbody; } set { _rigidbody = value; } }
    //索敵用ゲームオブジェクト
    public GameObject _searchObj;
    //狙う対象
    public GameObject _target;
    //シンプルアニメーション
    public SimpleAnimation _anim;

    //インタフェース用最大Hp取得
    public float MaxHitPoint { get { return _maxHp; } }
    //インタフェース用現在Hp取得
    public float HitPoint { get { return _hp; } }

    //ステータスのセット関数
    public void SetStatus (Enemy.Type type, float maxHp, float speed, float searchRange, float attackRange, float moveRange, float money) {
        //タイプセット
        _enemyType = type;
        //初期はアイドル
        _currentState = State.IDLE;
        //状態異常はなし
        _badState = BadState.NONE;
        //最大体力
        if (_maxHp == 0) { _maxHp = maxHp; }
        //体力
        _hp = _maxHp;
        //移動速度
        if (_moveSpeed == 0) { _moveSpeed = speed; }
        //索敵範囲
        if (_searchRange == 0) { _searchRange = searchRange; }
        //攻撃範囲
        if (_attackRange == 0) { _attackRange = attackRange; }
        //自由移動の幅
        if (_freeMoveRange == 0) { _freeMoveRange = moveRange; }
        //所持金の設定
        if (_money == 0) { _money = money; }
        //初期位置の記憶
        _startPosition = gameObject.transform.position;
        //animationSystem Set
        _anim = GetComponent<SimpleAnimation> ();
    }

    //ダメージを受ける
    public void TakeDamage (float damage) {
        if (_currentState == State.DEAD) return;
        _hp -= damage;

        if (_hp <= 0) {
            _currentState = State.DEAD;
            StartCoroutine (Dying ());
        }
    }

    //死亡コルーチン
    private IEnumerator Dying () {
        //Dead Animation
        _anim.CrossFade ("Dead", 0);
        //Wait Animation End 
        yield return new WaitForSeconds (2);
        //Object Release
        ObjectManager.Instance.ReleaseObject (gameObject);
    }

    public virtual void Discover (GameObject obj) { }

}