/// 近距離攻撃タイプの敵
/// Enemy of Close Combat Type
/// Athor： Yuhei Mastumura
/// Last edit date：2018/11/19
/// ★印は留学生に改変された部分

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatEnemy : Enemy {

    //TODO Enemy Performance
    //const float MAX_HP = 40.0f;
    //const float MOVE_SPEED = 3.0f;
    const float SEARCH_RANGE = 6.0f;
    const float ATTACK_RANGE = 4.0f;
    const float MOVE_RANGE = 3.0f;
    const float MONEY = 10.0f;
    const float PATIENCE_VALUE = 15.0f;

    //移動スクリプト
    EnemyMove _move;

    private bool _inMotion = false;

    [SerializeField]
    private List<GameObject> _weaponList;

    private Enemy.State _tmpState = Enemy.State.IDLE;

    // Use this for initialization
    public override void Init (Stats _stat) {
        _properties = _stat;

        //ステータスのセット
        SetStatus (Enemy.Type.MEEL, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //忍耐値のセット
        _patienceValue = PATIENCE_VALUE;
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove> ();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody> ();
        //索敵用オブジェクトの取得
        _searchObj = transform.Find ("SearchRange").gameObject;
        _searchObj.GetComponent<SearchPlayer> ().Initialize ();
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos ();
        //武器プレハブの取得
        SetWeapons ();
        _anim = GetComponent<SimpleAnimation>();
        _anim.CrossFade ("Idle", 0f);
        _animName = "Idle";

        _isLady = true;

    }

    // Update is called once per frame
    void Update () {

        //★ステータスの更新
        _status.UpdateStatMultiplyer (ref _properties);
        //★状態ダメージを受ける
        TakeDamage (_status.GetValue (EnumHolder.EffectType.TAKEDAMAGE));
        //開閉アニメーションの為の状態チェック
        StateCheck ();

        //Speed０（麻痺中は行動しない）
        if(_properties.SpeedMultiplyerProperties== 0)return;

        //被ダメアニメーション中は行動できない
        if (IsDamaged == true || _inMotion == true) return;

        switch (CurrentState) {

            case State.IDLE:
                //待機
                StartCoroutine (_move.Idle ());
                _anim.CrossFade ("Idle", 0);
                _animName = "Idle";
                break;

            case State.FREE:
                //自由移動
                _move.FreeMove ();
                _anim.CrossFade ("Move", 0.5f);
                _animName = "Move";
                break;

            case State.DISCOVERY:
                //プレイヤー追従
                _move.Dash2Player ();
                _anim.CrossFade ("Dash", 0.5f);
                _animName = "Dash";
                break;

            case State.RETURN:
                //初期位置に帰る
                _move.Return2FirstPos ();
                _anim.CrossFade ("Move", 0.5f);
                _animName = "Move";
                break;

            case State.ATTACK:
                //攻撃開始
                Attack ();
                break;

            default:
                break;

        }
    }

    //攻撃
    private void Attack () {
        //行動中はreturn
        if (IsAction || CurrentState == State.DEAD) return;
        //行動開始
        IsAction = true;

        //対象の方向を見る
        if (_target) {
            //対象の位置を取得
            Vector3 targetPos = _target.transform.position;
            //高さ合わせ
            targetPos.y = gameObject.transform.position.y;
            //相手の方向を見る。
            gameObject.transform.LookAt (targetPos);
        }

        _anim.CrossFade ("Idle", 0);

        int attackNum = Random.Range (1, 3);

        _anim.CrossFade ("Attack" + attackNum.ToString (), 0);

        _animName = "Attack" + attackNum.ToString ();

    }

    //発見時
    public override void Discover (GameObject obj) {
        if (CurrentState != Enemy.State.DEAD) {
            //Set Target
            _target = obj.gameObject;
            //Change State
            CurrentState = Enemy.State.DISCOVERY;
        }
    }

    private void SetWeapons () {
        _weaponList = new List<GameObject> ();

        List<GameObject> childList = GetAllChildren.GetAll (gameObject);

        foreach (GameObject obj in childList) {
            //child is your child transform

            //Make sure the target has components
            var hasEnemyWeapon = obj.GetComponent<EnemyWeapon> ();

            //If have a component
            if (hasEnemyWeapon != null) {
                _weaponList.Add (obj);
            }

        }

        childList.Clear ();
    }

    void StartHit () {
        _weaponList.ForEach (weapon => {
            //武器の当たり判定の実体化
            weapon.GetComponent<EnemyWeapon> ().ActiveCollision (true);
            //武器の既当たり判定をリセット
            weapon.GetComponent<EnemyWeapon> ().HashReset ();
            weapon.GetComponent<EnemyWeapon> ()._hitSE = "Melee_AttackHit";
        });
    }

    void EndHit () {
        //武器の判定を消す
        _weaponList.ForEach (weapon => weapon.GetComponent<EnemyWeapon> ().ActiveCollision (false));

        //行動終了
        IsAction = false;
    }

    void EndMotion () {
        _inMotion = false;
    }

    //状態変化（移動等）の確認（開閉アニメーションの為の措置）
    void StateCheck () {
        //一個前の状態と現在の状態が異なる場合
        if (_tmpState != CurrentState) {

            switch (_tmpState) {
                //待機状態からの変更の場合
                case Enemy.State.IDLE:
                    if (CurrentState == Enemy.State.DISCOVERY) {
                        _anim.CrossFade ("Close", 0.5f);
                        _animName = "Close";
                        _inMotion = true;
                    }
                    break;

                case Enemy.State.FREE:
                    if (CurrentState == Enemy.State.DISCOVERY) {
                        _anim.CrossFade ("Close", 0.5f);
                        _animName = "Close";
                        _inMotion = true;
                    }
                    break;

                case Enemy.State.ATTACK:
                    if (CurrentState == Enemy.State.DISCOVERY) {
                        _anim.CrossFade ("Close", 0.5f);
                        _animName = "Close";
                        _inMotion = true;
                    }
                    break;

                case Enemy.State.DISCOVERY:
                    if (CurrentState == Enemy.State.ATTACK) {
                        _anim.CrossFade ("Open", 0.5f);
                        _animName = "Open";
                        _inMotion = true;
                    }

                    if (CurrentState == Enemy.State.FREE) {
                        _anim.CrossFade ("Open", 0.5f);
                        _animName = "Open";
                        _inMotion = true;
                    }
                    break;

                default:
                    break;

            }

            _tmpState = CurrentState;
        }

    }


    

}