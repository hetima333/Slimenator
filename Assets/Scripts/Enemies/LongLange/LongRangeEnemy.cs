/// 遠距離攻撃タイプの敵
/// Enemy of Long Range Type
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/25
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

    //移動スクリプト
    EnemyMove _move;
    [SerializeField]
    float _outputDamage = 25;

    private GameObject _bullet;

    // Use this for initialization
    public override void Init(Stats _stat)
    {
        _properties = _stat;

        //ステータスのセット
        SetStatus(Enemy.Type.RANGE, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove>();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody>();
        _searchObj = transform.Find("SearchRange").gameObject;
        _searchObj.GetComponent<SearchPlayer>().Initialize();
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos();
        //弾オブジェクトのロード
        _bullet = Resources.Load("EnemyItem/EnemyBullet", typeof(GameObject)) as GameObject;

    }

    // Update is called once per frame
    void Update () {

        _status.UpdateStatMultiplyer(ref _properties);
        TakeDamage(_status.GetValue(EnumHolder.EffectType.TAKEDAMAGE));

        switch (CurrentState) {

            case State.IDLE:
                //待機
                StartCoroutine (_move.Idle ());
                _anim.CrossFade ("Idle", 0.5f);
                break;

            case State.FREE:
                //自由移動
                _move.FreeMove ();
                _anim.CrossFade ("Move", 0.5f);
                break;
            case State.DISCOVERY:
                //プレイヤー追従
                _move.Move2Player ();
                _anim.CrossFade ("Move", 0.5f);
                break;

            case State.RETURN:
                //初期位置に帰る
                _move.Return2FirstPos ();
                _anim.CrossFade ("Move", 0.5f);
                break;

            case State.ATTACK:
                //攻撃開始
                StartCoroutine (Attack ());
                break;
            default:
                break;

        }
    }

    //攻撃コルーチン
    private IEnumerator Attack () {
        //行動中はreturn
        if (IsAction || CurrentState == State.DEAD) yield break;
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

        //アニメーションとの時間調整
        yield return new WaitForSeconds (0.2f);

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

        //TODO行動終了までの時間経過
        yield return new WaitForSeconds (1);

        if (CurrentState == State.DEAD) yield break;

        _anim.CrossFade ("Idle", 0);
        //行動終了
        IsAction = false;

    }

    public override void Discover (GameObject obj) {
        if (CurrentState != Enemy.State.DEAD) {
            //Set Target
            _target = obj.gameObject;
            //Change State
            CurrentState = Enemy.State.DISCOVERY;
        }
    }

}