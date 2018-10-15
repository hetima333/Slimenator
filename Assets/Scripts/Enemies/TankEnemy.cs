/// Tankタイプの敵
/// Enemy of Tank Type
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/14

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{

    //移動スクリプト
    EnemyMove _move;

    int _comboCount = 1;

    // Use this for initialization
    void Start()
    {

        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove>();
        //リジットボディの取得
        _rigidbody = GetComponent<Rigidbody>();
        //索敵用コライダーの設定
        _sphereCol = GetComponent<SphereCollider>();
        //TriggerOn
        _sphereCol.isTrigger = true;
        //範囲設定
        _sphereCol.radius = _searchRange;
        //初期位置を記憶
        _staetPosition = gameObject.transform.position;
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos();

    }

    // Update is called once per frame
    void Update()
    {

        switch (_currentState)
        {

            case State.IDLE:
                //待機
                StartCoroutine(_move.Idle());
                break;

            case State.FREE:
                //自由移動
                _move.FreeMove();
                break;
            case State.DISCOVERY:
                //プレイヤー追従
                _move.Move2Player();
                break;

            case State.RETURN:
                //初期位置に帰る
                _move.Return2FirstPos();
                break;

            case State.ATTACK:
                //攻撃開始
                StartCoroutine(Attack());
                break;

            default:
                break;

        }
    }



    //攻撃コルーチン
    private IEnumerator Attack()
    {
        //行動中はreturn
        if (_isAction) yield break;
        //行動開始
        _isAction = true;

        //TODO　攻撃
        Debug.Log("Combo"+_comboCount);



        //TODO行動終了までの時間経過
        yield return new WaitForSeconds(1);

        //コンボのカウント増加
        _comboCount++;
        if (_comboCount > 3)
        {
            _comboCount = 1;
        }

        //行動終了
        _isAction = false;

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Targetの設定
            _target = col.gameObject;
            //発見状態になる
            _currentState = State.DISCOVERY;
        }
    }

    //戦闘範囲離脱時の処理
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Targetの設定
            _target = null;
            //通常状態になる
            _currentState = State.FREE;
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

