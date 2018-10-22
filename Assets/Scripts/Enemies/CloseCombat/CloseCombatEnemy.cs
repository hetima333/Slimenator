/// 近距離攻撃タイプの敵
/// Enemy of Close Combat Type
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CloseCombatEnemy : Enemy
{
    //TODO Enemy Performance
    const float MAX_HP = 40.0f;
    const float MOVE_SPEED = 3.0f;
    const float SEARCH_RANGE = 3.5f;
    const float ATTACK_RANGE = 2.0f;
    const float MOVE_RANGE = 3.0f;
    const float MONEY = 10.0f;

    //移動スクリプト
    EnemyMove _move;

    [SerializeField]
    float _outputDamage = 25;

    // Use this for initialization
    void Start()
    {
        //ステータスのセット
        SetStatus(MAX_HP, MOVE_SPEED, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove>();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody>();
        //索敵用コライダーの設定
        SphereColliderProperties = GetComponent<SphereCollider>();
        //TriggerOn
        SphereColliderProperties.isTrigger = true;
        //範囲設定
        SphereColliderProperties.radius = _searchRange;
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos();

    }

    // Update is called once per frame
    void Update()
    {

        switch (CurrentState)
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
                _move.Dash2Player();
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
        if (IsAction) yield break;
        //行動開始
        IsAction = true;

        //対象の方向を見る
        if (_target)
        {
            //対象の位置を取得
            Vector3 targetPos = _target.transform.position;
            //高さ合わせ
            targetPos.y = gameObject.transform.position.y;
            //相手の方向を見る。
            gameObject.transform.LookAt(targetPos);
        }

        //TODO　攻撃
        Debug.Log("Attack");

        //TODO行動終了までの時間経過
        yield return new WaitForSeconds(1);

        //行動終了
        IsAction = false;

    }


}
