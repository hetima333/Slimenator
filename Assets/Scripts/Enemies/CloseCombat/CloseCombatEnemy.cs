/// 近距離攻撃タイプの敵
/// Enemy of Close Combat Type
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CloseCombatEnemy : Enemy
{

    //TODO Enemy Performance
    //const float MAX_HP = 40.0f;
    //const float MOVE_SPEED = 3.0f;
    const float SEARCH_RANGE = 6.0f;
    const float ATTACK_RANGE = 4f;
    const float MOVE_RANGE = 3.0f;
    const float MONEY = 10.0f;

    //移動スクリプト
    EnemyMove _move;

    [SerializeField]
    float _outputDamage = 25;

    [SerializeField]
    private List<GameObject> _weaponList;

    // Use this for initialization
    public override void Init (Stats _stat) {
        _properties = _stat;

        //ステータスのセット
        SetStatus (Enemy.Type.MEEL, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
        //移動コンポーネントの取得
        _move = GetComponent<EnemyMove> ();
        //リジットボディの取得
        RigidbodyProperties = GetComponent<Rigidbody> ();
        _searchObj = transform.Find ("SearchRange").gameObject;
        _searchObj.GetComponent<SearchPlayer> ().Initialize ();
        //自由移動ポジション設定
        _freeMovePosition = _move.SetMovePos ();
        //武器プレハブの取得
        SetWeapons ();

    }

    // Update is called once per frame
    void Update () {

        _status.UpdateStatMultiplyer(ref _properties);
        TakeDamage(_status.GetValue(EnumHolder.EffectType.TAKEDAMAGE));

        switch (CurrentState) {

            case State.IDLE:
                //待機
                StartCoroutine (_move.Idle ());
                _anim.CrossFade ("Idle", 0);
                break;

            case State.FREE:
                //自由移動
                _move.FreeMove ();
                _anim.CrossFade ("Move", 0.5f);
                break;

            case State.DISCOVERY:
                //プレイヤー追従
                _move.Dash2Player ();
                _anim.CrossFade ("Dash", 0.5f);
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

        int attackNum = Random.Range (1, 3);

        _anim.CrossFade ("Attack" + attackNum.ToString (), 0);

        //TODO行動終了までの時間経過
        yield return new WaitForSeconds (3);

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
        });
    }

    void EndHit () {
        //武器の判定を消す
        _weaponList.ForEach (weapon => weapon.GetComponent<EnemyWeapon> ().ActiveCollision (false));
    }
}