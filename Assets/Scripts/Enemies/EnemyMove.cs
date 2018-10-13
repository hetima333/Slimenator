/// 敵の移動
/// Enemies Move
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/12
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {


    Enemy _enemy;


    // Use this for initialization
    void Awake () {
        _enemy = GetComponent<Enemy>();
    }
	
	// Update is called once per frame
	void Update () {
        //移動速度のリセット
        _enemy._rigidbody.velocity = Vector3.zero;

    }


    //待機コルーチン
    public IEnumerator Idle()
    {
        //TODO 指定時間待機
        yield return new WaitForSeconds(2);

        if (_enemy._currentState != Enemy.State.IDLE) yield break;
        //目的地の再取得
        _enemy._freeMovePosition = SetMovePos();
        //自由移動に移行
        _enemy._currentState = Enemy.State.FREE;
    }


    //自由移動
    public void FreeMove()
    {

        //現在座標を取得
        Vector3 pos = gameObject.transform.position;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._freeMovePosition) >= 0.1f)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(_enemy._freeMovePosition);
            //移動
            _enemy._rigidbody.velocity = transform.forward * _enemy._moveSpeed;

            if (Vector3.Distance(pos, _enemy._staetPosition) >= _enemy._freeMoveRange)
            {
                //戻り状態になる
                //目的地の再取得
                _enemy._freeMovePosition = SetMovePos();
                _enemy._currentState = Enemy.State.RETURN;
            }

        }
        else
        {
            //待機状態になる。
            _enemy._currentState = Enemy.State.IDLE;
        }

    }

    //プレイヤーに向かっての移動
    public void Move2Player()
    {

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
            _enemy._rigidbody.velocity = transform.forward * _enemy._moveSpeed;
        }
        else
        {
            //攻撃状態に移行
            _enemy._currentState = Enemy.State.ATTACK;
        }

    }

    //初期位置に戻る
    public void Return2FirstPos()
    {
        //現在座標を取得
        Vector3 pos = gameObject.transform.position;

        //現在座標と目的座標の差が0.1f以上
        if (Vector3.Distance(pos, _enemy._staetPosition) >= 0.1f)
        {
            //向かう場所の方向を見る
            gameObject.transform.LookAt(_enemy._staetPosition);
            //移動
            _enemy._rigidbody.velocity = transform.forward * _enemy._moveSpeed;
        }
        else
        {          
            _enemy._currentState = Enemy.State.IDLE;
        }
    }


    
    //目的地のセット
    public Vector3 SetMovePos()
    {

        //移動先の座標を求める（Find the coordinates to move）
        //X軸移動量の設定
        float randomX = Random.Range(-_enemy._freeMoveRange, _enemy._freeMoveRange);
        //Z軸移動量の設定
        float randomZ = Random.Range(-_enemy._freeMoveRange, _enemy._freeMoveRange);
        //次の移動座標の設定
        Vector3 nextPos = new Vector3(randomX, 1, randomZ);

        return nextPos;
    }
}
