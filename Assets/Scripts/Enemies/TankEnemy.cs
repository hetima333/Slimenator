/// Tankタイプの敵
/// Enemy of Tank Type
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/14

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    //TODO Enemy Performance
    const float MAX_HP = 30.0f;
    const float MOVE_SPEED = 1.0f;
    const float SEARCH_RANGE = 3.0f;
    const float ATTACK_RANGE = 2.0f;
    const float MOVE_RANGE = 2.0f;

    //移動スクリプト
    EnemyMove _move;

    int _comboCount = 1;

    [SerializeField]
    GameObject _shockWave;

    // Use this for initialization
    void Start()
    {
        //ステータスのセット
        SetStatus(MAX_HP, MOVE_SPEED, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE);
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
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos();
        //衝撃波オブジェクトのロード
        _shockWave = Resources.Load("EnemyItem/ShockWave", typeof(GameObject)) as GameObject;

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
        Debug.Log("Combo"+_comboCount);

        if (_comboCount == 3)
        {
            if (_shockWave)
            {
                GameObject shockWave = Instantiate(_shockWave) as GameObject;

                Vector3 ShockPos = gameObject.transform.position + transform.forward;
                ShockPos.y = 0.1f;
                shockWave.transform.position = ShockPos;    
            }
        }

        



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

