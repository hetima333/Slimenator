/// 敵の移動
/// Enemies Move
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/25
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    //ダッシュ倍率
    const float DASH_SPEED = 2.0f;
    const float IDLE_TIME = 2.0f;
    const float THINK_TIME = 3.0f;
    Enemy _enemy;

    float _thinkTime = THINK_TIME;
    //目的地のと距離
    float _distance;

    // Use this for initialization
    void Awake () {
        _enemy = GetComponent<Enemy> ();
    }

    void Update () {
        RotFixed ();
    }

    //待機コルーチン
    public IEnumerator Idle () {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //思考時間リセット
        _thinkTime = THINK_TIME;
        //TODO 指定時間待機
        yield return new WaitForSeconds (IDLE_TIME);

        if (_enemy.CurrentState != Enemy.State.IDLE) yield break;
        //目的地の再取得
        _enemy._freeMovePosition = SetMovePos ();
        //自由移動に移行
        _enemy.CurrentState = Enemy.State.FREE;

    }

    //自由移動
    public void FreeMove () {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //距離を算出
        _distance = (pos - _enemy._freeMovePosition).sqrMagnitude;
        //現在座標と目的座標の差が0.1f以上
        if (_distance > 0.1f) {
            //TODO 
            _thinkTime -= Time.deltaTime;
            if (_thinkTime <= 0) {
                //待機状態になる。
                _enemy.CurrentState = Enemy.State.IDLE;
            }
            //向かう場所の方向を見る
            gameObject.transform.LookAt (_enemy._freeMovePosition);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;

            if (_distance >= Mathf.Pow (_enemy._freeMoveRange, 2)) {
                //戻り状態になる
                //目的地の再取得
                _enemy._freeMovePosition = SetMovePos ();
                _enemy.CurrentState = Enemy.State.RETURN;
            }

        } else {
            //待機状態になる
            _enemy.CurrentState = Enemy.State.IDLE;
        }

    }

    //プレイヤーに向かっての移動
    public void Move2Player () {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        if (!_enemy._target) {
            _enemy.CurrentState = Enemy.State.FREE;
            return;
        }
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 targetPos = _enemy._target.transform.position;
        //高さ合わせ
        targetPos.y = gameObject.transform.position.y;
        //距離を算出
        _distance = (pos - targetPos).sqrMagnitude;

        //現在座標と目的座標の差が攻撃範囲以上
        if (_distance >= Mathf.Pow (_enemy._attackRange, 2)) {
            //向かう場所の方向を見る
            gameObject.transform.LookAt (targetPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;
        } else {
            //攻撃状態に移行
            _enemy.CurrentState = Enemy.State.ATTACK;
        }
    }

    public void Dash2Player () {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //対象がいない場合は終了
        if (!_enemy._target) {
            _enemy.CurrentState = Enemy.State.FREE;
            return;
        }
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 targetPos = _enemy._target.transform.position;
        //高さ合わせ
        targetPos.y = gameObject.transform.position.y;
        //距離を算出
        _distance = (pos - targetPos).sqrMagnitude;

        //攻撃範囲に入ったら
        if (_distance >= Mathf.Pow (_enemy._attackRange, 2)) {
            //向かう場所の方向を見る
            gameObject.transform.LookAt (targetPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed * DASH_SPEED;
        } else {
            //攻撃状態に移行
            _enemy.CurrentState = Enemy.State.ATTACK;
        }
    }

    //初期位置に戻る
    public void Return2FirstPos () {

        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 startPos = _enemy._startPosition;
        //高さ合わせ
        startPos.y = gameObject.transform.position.y;

        //距離を算出
        _distance = (pos - startPos).sqrMagnitude;

        //現在座標と目的座標の差が0.1f以上
        if (_distance >= 0.5f) {
            //向かう場所の方向を見る
            gameObject.transform.LookAt (startPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;
        } else {
            _enemy.CurrentState = Enemy.State.IDLE;
        }

    }

    //目的地のセット
    public Vector3 SetMovePos () {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;

        //移動先の座標を求める（Find the coordinates to move）
        //X軸移動量の設定
        float randomX = Random.Range (-_enemy._freeMoveRange, _enemy._freeMoveRange);
        float nextX = gameObject.transform.position.x + randomX;
        //Z軸移動量の設定
        float randomZ = Random.Range (-_enemy._freeMoveRange, _enemy._freeMoveRange);
        float nextZ = gameObject.transform.position.z + randomZ;
        //次の移動座標の設定
        Vector3 nextPos = new Vector3 (nextX, gameObject.transform.position.y, nextZ);

        return nextPos;
    }

    void RotFixed () {
        //モデルの中心軸ずれによる傾きの防止
        var rot = gameObject.transform.rotation;
        if (rot.x != 0 || rot.z != 0) {
            rot.x = 0;
            rot.z = 0;
            gameObject.transform.rotation = rot;
        }

    }
}