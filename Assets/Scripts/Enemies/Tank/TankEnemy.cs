/// Tankタイプの敵
/// Enemy of Tank Type
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/31

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy {
    //TODO Enemy Performance
    //const float MAX_HP = 250.0f;
    //const float MOVE_SPEED = 2.0f;
    const float SEARCH_RANGE = 11.0f;
    const float ATTACK_RANGE = 4.0f;
    const float MOVE_RANGE = 4.0f;
    const float MONEY = 150.0f;
    const float ERROR_RANGE = 10.0f;

    //移動スクリプト
    EnemyMove _move;

    private int _comboCount = 0;

    private GameObject _shockWave;

    private bool _isSleeping = true;

    [SerializeField]
    private List<GameObject> _weaponList;

    [SerializeField]
    private float[] _comboDamage = { 10, 15, 20 };

    private float[] _comboDelay = { 1.5f, 0.8f, 1.7f };

    public override void Init (Stats _stat) {
        _properties = _stat;

        //ステータスのセット
        SetStatus (Enemy.Type.TANK, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
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

        _status.UpdateStatMultiplyer (ref _properties);
        TakeDamage (_status.GetValue (EnumHolder.EffectType.TAKEDAMAGE));

        if (!_isSleeping) {
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
                    _move.Move2Player ();
                    _anim.CrossFade ("Move", 0.5f);
                    _animName = "Move";
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

    }

    //攻撃コルーチン
    private void Attack () {
        //行動中はreturn
        if (IsAction || CurrentState == State.DEAD) return;
        //攻撃範囲から出れば攻撃をやめる
        if ((gameObject.transform.position - _target.transform.position).sqrMagnitude > Mathf.Pow (_attackRange, 2) + ERROR_RANGE) {
            CurrentState = State.DISCOVERY;
            _comboCount = 0;
            return;
        }
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

        _anim.CrossFade ("Attack" + (_comboCount + 1).ToString (), 0);

        _animName = "Attack" + (_comboCount + 1).ToString ();

        //コンボのカウント増加
        _comboCount++;
        if (_comboCount > 2) {
            _comboCount = 0;
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

    public override void Discover (GameObject obj) {
        //Set Target
        _target = obj;
        if (_isSleeping) {
            StartCoroutine (WakeUp ());
        } else if (CurrentState != State.DEAD && !IsAction) {
            CurrentState = State.DISCOVERY;
            _comboCount = 0;
        }
    }

    private IEnumerator WakeUp () {
        //起き上がりアニメーション
        //WakeUp Animation
        _anim.CrossFade ("WakeUp", 0.5f);

        //起き上がるまで待つ
        //wait for end wakeup
        yield return new WaitForSeconds (6);

        _isSleeping = false;
        //Change State
        CurrentState = State.DISCOVERY;
    }

    void StartHit () {
        _weaponList.ForEach (weapon => {
            //武器のダメージセット
            weapon.GetComponent<EnemyWeapon> ().SetDamage (_comboDamage[_comboCount]);
            //武器の当たり判定の実体化
            weapon.GetComponent<EnemyWeapon> ().ActiveCollision (true);
            //武器の既当たり判定をリセット
            weapon.GetComponent<EnemyWeapon> ().HashReset ();
        });
    }

    void EndHit () {
        //武器の判定を消す
        _weaponList.ForEach (weapon => weapon.GetComponent<EnemyWeapon> ().ActiveCollision (false));

        //行動終了
        IsAction = false;
    }

}