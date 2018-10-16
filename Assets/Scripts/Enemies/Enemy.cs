/// 敵の基本クラス
/// Base class of enemies
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/11
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
    public State _currentState;
    //体力
    public float _hp;
    //移動速度
    public float _moveSpeed;
    //索敵範囲
    public float _searchRange;
    //攻撃範囲
    public float _attackRange;
    //自由移動の幅
    public float _freeMoveRange;
    //自由移動の目標座標
    public Vector3 _freeMovePosition;
    //初期座標
    public Vector3 _staetPosition;
    //行動中か？
    public bool _isAction;
    //移動用リジットボディ
    public Rigidbody _rigidbody;
    //索敵用コライダー
    public SphereCollider _sphereCol;
    //狙う対象
    public GameObject _target;

    //ステータスのセット関数
    public void SetStatus(float hp,float speed,float searchRange,float attackRange,float moveRange)
    {
        //初期はアイドル
        _currentState = State.IDLE;
        //体力
        _hp = hp;
        //移動速度
        _moveSpeed = speed;
        //索敵範囲
        _searchRange = searchRange;
        //攻撃範囲
        _attackRange = attackRange;
        //自由移動の幅
        _freeMoveRange = moveRange;
        //初期位置の記憶
        _staetPosition = gameObject.transform.position;
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
    IEnumerator Dying()
    {

        Debug.Log("Dead");

        yield return new WaitForSeconds(2);

        Destroy(gameObject);

    }

}
