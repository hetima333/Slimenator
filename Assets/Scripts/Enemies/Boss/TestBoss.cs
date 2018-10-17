using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : Enemy {

    EnemyMove _move;

    //TODO Boss Performance
    const float MAX_HP = 3000.0f;
    const float MOVE_SPEED = 2.0f;
    const float SEARCH_RANGE =3.0f;
    const float ATTACK_RANGE = 9.0f;
    const float MOVE_RANGE = 10.0f;
    const float MONEY = 2000.0f;


    private GameObject _bullet;

    private GameObject _shockWave;


    float _distance = 0;

    // Use this for initialization
    void Start () {
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

        //衝撃波オブジェクトのロード
        _shockWave = Resources.Load("EnemyItem/ShockWave", typeof(GameObject)) as GameObject;

        //弾オブジェクトのロード
        _bullet = Resources.Load("EnemyItem/EnemyBullet", typeof(GameObject)) as GameObject;

    }
	
	// Update is called once per frame
	void Update () {

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


   

        _distance = Vector3.Distance(_target.transform.position, gameObject.transform.position);

        if (_distance <= 6.0f)
        {

            //TODO　攻撃
            Debug.Log("Jump");
            if (_shockWave)
            {
                GameObject shockWave = Instantiate(_shockWave) as GameObject;
                shockWave.transform.position = gameObject.transform.position;
                shockWave.GetComponent<ShockWave>().SetDamage(10);
                shockWave.GetComponent<ShockWave>().SetScale(20);
              
                
            }

        }
        else
        {

            Debug.Log("Fire");
            if (_bullet)
            {
                //make bullet 
                GameObject bullet = Instantiate(_bullet) as GameObject;
                //set bullet damage
                bullet.GetComponent<EnemyBullet>().SetDamage(10);

                bullet.GetComponent<EnemyBullet>().SetScale(1);
                //set bullet position
                bullet.transform.position = gameObject.transform.position + transform.forward;
                //set bullet speed(TODO)
                bullet.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * 10;
            }
        }


        //TODO行動終了までの時間経過
        yield return new WaitForSeconds(1);

        //コンボのカウント増加
        

        //行動終了
        IsAction = false;

    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Targetの設定
            _target = col.gameObject;
            //発見状態になる
            CurrentState = State.DISCOVERY;
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
            CurrentState = State.FREE;
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
