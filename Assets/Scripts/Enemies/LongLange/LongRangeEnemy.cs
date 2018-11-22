/// 遠距離攻撃タイプの敵
/// Enemy of Long Range Type
/// Athor：Yuhei Mastumura
/// Last edit date：2018/11/15
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : Enemy {
    //TODO Enemy Performance
    //const float MAX_HP = 100.0f;
    //const float MOVE_SPEED = 2.5f;
    const float SEARCH_RANGE = 14.0f;
    const float ATTACK_RANGE = 10.0f;
    const float MOVE_RANGE = 10.0f;
    const float MONEY = 50.0f;
    const float PATIENCE_VALUE = 15.0f;

    //移動スクリプト
    EnemyMove _move;
    [SerializeField]
    float _outputDamage;

    private GameObject _bullet;

    // Use this for initialization
    public override void Init (Stats _stat) {
        _properties = _stat;

        //ステータスのセット
        SetStatus (Enemy.Type.RANGE, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //忍耐値のセット
        _patienceValue = PATIENCE_VALUE;
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove> ();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody> ();
        _searchObj = transform.Find ("SearchRange").gameObject;
        _searchObj.GetComponent<SearchPlayer> ().Initialize ();
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos ();
        //弾オブジェクトのロード
        _bullet = Resources.Load ("EnemyItem/EnemyBullet", typeof (GameObject)) as GameObject;

    }

    // Update is called once per frame
    void Update () {

        _status.UpdateStatMultiplyer (ref _properties);
        TakeDamage (_status.GetValue (EnumHolder.EffectType.TAKEDAMAGE));

        //被ダメアニメーション中は行動できない
        if (IsDamaged == true) return;

        switch (CurrentState) {

            case State.IDLE:
                //待機
                IsAction = false;
                StartCoroutine (_move.Idle ());
                _anim.CrossFade ("Idle", 0.5f);
                _animName = "Idle";
                break;

            case State.FREE:
                //自由移動
                IsAction = false;
                _move.FreeMove ();
                _anim.CrossFade ("Move", 0.5f);
                _animName = "Move";
                break;
            case State.DISCOVERY:
                //プレイヤー追従
                IsAction = false;
                _move.Move2Player ();
                _anim.CrossFade ("Move", 0.5f);
                _animName = "Move";
                break;

            case State.RETURN:
                //初期位置に帰る
                IsAction = false;
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

    //攻撃コルーチン
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

        //遠距離攻撃
        _anim.CrossFade ("Attack", 0.5f);
        _animName = "Attack";
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

    //攻撃判定開始（AnimationEvent用）
    void StartHit () {
        if (_bullet) {
            //make bullet 
            GameObject bullet = ObjectManager.Instance.InstantiateWithObjectPooling (_bullet) as GameObject;
            //set bullet damage
            bullet.GetComponent<EnemyBullet> ().SetDamage (_outputDamage);
            //set bullet position
            bullet.transform.position = gameObject.transform.position + transform.forward;
            //set bullet speed(TODO)
            bullet.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * 10;
        }
    }

    //攻撃判定終了（AnimationEvent用）
    void EndHit () {

        if (CurrentState == State.DEAD) return;
        //待機アニメーション
        _anim.CrossFade ("Idle", 0);
        //行動終了
        IsAction = false;
    }

}