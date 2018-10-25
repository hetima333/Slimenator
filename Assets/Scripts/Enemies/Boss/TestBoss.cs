/// ボスのテストタイプ
/// Boss Test type
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : Enemy {

    EnemyMove _move;

    //TODO Boss Performance
    const float MAX_HP = 3000.0f;
    const float MOVE_SPEED = 2.0f;
    const float SEARCH_RANGE = 3.0f;
    const float ATTACK_RANGE = 9.0f;
    const float MOVE_RANGE = 10.0f;
    const float MONEY = 2000.0f;

    //Object for long range attack
    private GameObject _bullet;

    //Object for short range attack
    private GameObject _shockField;

    // Use this for initialization
    void Start () {
        //ステータスのセット
        SetStatus (MAX_HP, MOVE_SPEED, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove> ();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody> ();
        //索敵用コライダーの設定
        SphereColliderProperties = GetComponent<SphereCollider> ();
        //TriggerOn
        SphereColliderProperties.isTrigger = true;
        //範囲設定
        SphereColliderProperties.radius = _searchRange;
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos ();

        //衝撃波オブジェクトのロード
        _shockField = Resources.Load ("EnemyItem/ShockField", typeof (GameObject)) as GameObject;

        //弾オブジェクトのロード
        _bullet = Resources.Load ("EnemyItem/EnemyBullet", typeof (GameObject)) as GameObject;

    }

    // Update is called once per frame
    void Update () {

        switch (CurrentState) {

            case State.IDLE:
                //待機
                StartCoroutine (_move.Idle ());
                break;

            case State.FREE:
                //自由移動
                _move.FreeMove ();
                break;
            case State.DISCOVERY:
                //プレイヤー追従
                _move.Move2Player ();
                break;

            case State.RETURN:
                //初期位置に帰る
                _move.Return2FirstPos ();
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
        if (IsAction) yield break;
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

        //距離を算出
        float distance = Vector3.Distance (_target.transform.position, gameObject.transform.position);

        if (distance <= 6.0f) {
            if (_shockField) {
                GameObject shockField = Instantiate (_shockField) as GameObject;
                shockField.transform.position = gameObject.transform.position;
                shockField.GetComponent<ShockField> ().SetDamage (10);
                shockField.GetComponent<ShockField> ().SetScale (20);
            }
        } else {
            if (_bullet) {
                //make bullet 
                GameObject bullet = Instantiate (_bullet) as GameObject;
                //set bullet damage
                bullet.GetComponent<EnemyBullet> ().SetDamage (10);
                bullet.GetComponent<EnemyBullet> ().SetScale (1);
                //set bullet position
                bullet.transform.position = gameObject.transform.position + transform.forward;
                //set bullet speed(TODO)
                bullet.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * 10;
            }
        }

        //TODO行動終了までの時間経過
        yield return new WaitForSeconds (1);
        //行動終了
        IsAction = false;
    }
}