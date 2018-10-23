/// 敵の移動
/// Enemies Move
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/18
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

    //ダッシュ倍率
    const float DASH_SPEED = 2.0f;

    Enemy _enemy;

    // Use this for initialization
    void Awake ()
    {
        _enemy = GetComponent<Enemy>();
    }
	
	// Update is called once per frame


    //待機コルーチン
    public IEnumerator Idle()
    {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //TODO 指定時間待機
        yield return new WaitForSeconds(2);

        if (_enemy.CurrentState != Enemy.State.IDLE) yield break;
        //目的地の再取得
        _enemy._freeMovePosition = SetMovePos();
        //自由移動に移行
        _enemy.CurrentState = Enemy.State.FREE;
    }


    //自由移動
    public void FreeMove()
    {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._freeMovePosition) >= 0.1f)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(_enemy._freeMovePosition);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;

            if (Vector3.Distance(pos, _enemy._staetPosition) >= _enemy._freeMoveRange)
            {
                //戻り状態になる
                //目的地の再取得
                _enemy._freeMovePosition = SetMovePos();
                _enemy.CurrentState = Enemy.State.RETURN;
            }

        }
        else
        {
            //待機状態になる。
            _enemy.CurrentState = Enemy.State.IDLE;
        }

        //モデルの中心軸ずれによる傾きの防止
        var rot = gameObject.transform.rotation;
        rot.x = 0;
        gameObject.transform.rotation = rot;

    }

    //プレイヤーに向かっての移動
    public void Move2Player()
    {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        if (!_enemy._target) return;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 targetPos = _enemy._target.transform.position;
        //高さ合わせ
        targetPos.y = gameObject.transform.position.y;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._target.transform.position) >= _enemy._attackRange)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(targetPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;
        }
        else
        {
            //攻撃状態に移行
            _enemy.CurrentState = Enemy.State.ATTACK;
        }
        //モデルの中心軸ずれによる傾きの防止
        var rot = gameObject.transform.rotation;
        rot.x = 0;
        gameObject.transform.rotation = rot;
    }


    public void Dash2Player()
    {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        if (!_enemy._target) return;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 targetPos = _enemy._target.transform.position;
        //高さ合わせ
        targetPos.y = gameObject.transform.position.y;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._target.transform.position) >= _enemy._attackRange)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(targetPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed * DASH_SPEED;
        }
        else
        {
            //攻撃状態に移行
            _enemy.CurrentState = Enemy.State.ATTACK;
        }
        //モデルの中心軸ずれによる傾きの防止
        var rot = gameObject.transform.rotation;
        rot.x = 0;
        gameObject.transform.rotation = rot;

    }

    //初期位置に戻る
    public void Return2FirstPos()
    {

        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;
        //対象の座標取得
        Vector3 startPos = _enemy._staetPosition;
        //高さ合わせ
        startPos.y = gameObject.transform.position.y;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._staetPosition) >= 0.5f)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(startPos);
            //移動
            _enemy.RigidbodyProperties.velocity = transform.forward * _enemy.Speed;
        }
        else
        {          
            _enemy.CurrentState = Enemy.State.IDLE;
        }
        //モデルの中心軸ずれによる傾きの防止
        var rot = gameObject.transform.rotation;
        rot.x = 0;
        gameObject.transform.rotation = rot;
    }


    
    //目的地のセット
    public Vector3 SetMovePos()
    {
        //移動速度のリセット
        _enemy.RigidbodyProperties.velocity = Vector3.zero;

        //移動先の座標を求める（Find the coordinates to move）
        //X軸移動量の設定
        float randomX = Random.Range(-_enemy._freeMoveRange, _enemy._freeMoveRange);

        float nextX = gameObject.transform.position.x + randomX;
        //Z軸移動量の設定
        float randomZ = Random.Range(-_enemy._freeMoveRange, _enemy._freeMoveRange);

        float nextZ = gameObject.transform.position.z + randomZ;
        //次の移動座標の設定
        Vector3 nextPos = new Vector3(nextX, gameObject.transform.position.y, nextZ);

        return nextPos;
    }
}
