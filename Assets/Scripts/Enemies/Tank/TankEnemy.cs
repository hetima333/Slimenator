/// Tankタイプの敵
/// Enemy of Tank Type
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/31
/// ★印は留学生に改変された部分

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy {
    //TODO Enemy Performance
    const float SEARCH_RANGE = 11.0f;
    const float ATTACK_RANGE = 4.0f;
    const float MOVE_RANGE = 4.0f;
    const float MONEY = 150.0f;
    const float ERROR_RANGE = 9.5f;
    const float PATIENCE_VALUE = 15.0f;

    //移動スクリプト
    EnemyMove _move;

    private int _comboCount = 0;

    private GameObject _shockWave;

    private bool _isSleeping = true;

    [SerializeField]
    private List<GameObject> _weaponList;

    [SerializeField]
    private float[] _comboDamage = { 10, 15, 20 };

    public override void Init (Stats _stat) {
        _properties = _stat;
        //ステータスのセット
        SetStatus (Enemy.Type.TANK, MaxHitPoint, Speed, SEARCH_RANGE, ATTACK_RANGE, MOVE_RANGE, MONEY);
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
        _anim.CrossFade ("Sleep", 0f);
        _animName = "Sleep";
        _isLady = true;
    }

    // Update is called once per frame
    void Update () {

        //★ステータスの更新
        _status.UpdateStatMultiplyer (ref _properties);
        //★状態ダメージを受ける
        TakeDamage (_status.GetValue (EnumHolder.EffectType.TAKEDAMAGE));

        //Speed０（麻痺中は行動しない）
        if(_properties.SpeedMultiplyerProperties== 0)return;
        //被ダメアニメーション中は行動できない
        if (IsDamaged == true) return;

        if (!_isSleeping) {
            switch (CurrentState) {

                case State.IDLE:
                    //待機
                    StartCoroutine (_move.Idle ());
                    if(_animName != "Idle")
                    {
                        _anim.CrossFade ("Idle", 0.5f);
                        _animName = "Idle";
                    }
                    break;

                case State.FREE:
                    //自由移動
                    _move.FreeMove ();
                    if(_animName != "Move")
                    {
                        _anim.CrossFade ("Move", 0.5f);
                        _animName = "Move";
                    }
                    break;

                case State.DISCOVERY:
                    //プレイヤー追従
                    _move.Move2Player ();
                    if(_animName != "Move")
                    {
                        _anim.CrossFade ("Move", 0.5f);
                        _animName = "Move";
                    }
                    break;

                case State.RETURN:
                    //初期位置に帰る
                    _move.Return2FirstPos ();
                    if(_animName != "Move")
                    {
                        _anim.CrossFade ("Move", 0.5f);
                        _animName = "Move";
                    }
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
        //攻撃アニメーションの再生
        _anim.CrossFade ("Attack" + (_comboCount + 1).ToString (), 0);
        _animName = "Attack" + (_comboCount + 1).ToString ();

        //コンボのカウント増加
        _comboCount++;
        if (_comboCount > 2) {
            _comboCount = 0;
        }
    }

    //所持している武器の取得
    private void SetWeapons () {
        //武器リスト作成
        _weaponList = new List<GameObject> ();
        //子オブジェクトのリスト作成
        List<GameObject> childList = GetAllChildren.GetAll (gameObject);

        foreach (GameObject obj in childList) {

            //EnemyWeaponのコンポーネントを持っているか
            //Make sure the target has components
            var hasEnemyWeapon = obj.GetComponent<EnemyWeapon> ();

            //コンポーネントを持っている場合
            //If have a component
            if (hasEnemyWeapon != null) {
                //武器リストにオブジェクトを登録する
                _weaponList.Add (obj);
            }

        }
        //子オブジェクトのリストをクリア
        childList.Clear ();
    }

    //発見時
    public override void Discover (GameObject obj) {
    if (CurrentState == Enemy.State.DEAD) return;

        //Set Target
        _target = obj;
        //行動終了
        IsAction = false;
        //寝ている場合
        if (_isSleeping) {
            //起き上がる
            WakeUp ();
        } else if (CurrentState != State.DEAD) {
            //発見状態にする
            CurrentState = State.DISCOVERY;
            _comboCount = 0;
        }
    }

    private void WakeUp () {
        //起き上がりアニメーション
        //WakeUp Animation
        _anim.CrossFade ("WakeUp", 0f);
        _animName = "WakeUp";
    }

    //攻撃判定開始（AnimationEvent用）
    void StartHit () {
        if (CurrentState == Enemy.State.DEAD) return;

        //所持している武器に対しての更新
        _weaponList.ForEach (weapon => {
            //武器をスイングする音
            AudioManager.Instance.PlaySE("Tank_Swing");
            //武器のダメージセット
            weapon.GetComponent<EnemyWeapon> ().SetDamage (_comboDamage[_comboCount]);
            //武器の当たり判定の実体化
            weapon.GetComponent<EnemyWeapon> ().ActiveCollision (true);
            //武器の既当たり判定をリセット
            weapon.GetComponent<EnemyWeapon> ().HashReset ();

        //コンボ数に応じたSEをセット
        if(_comboCount<1){
            weapon.GetComponent<EnemyWeapon> ()._hitSE ="Tank_Hit"+(_comboCount+1).ToString();
        }
        else{
            weapon.GetComponent<EnemyWeapon> ()._hitSE ="Tank_Hit"+(_comboCount).ToString();
        }
    
        });
    }

    //攻撃判定終了（AnimationEvent用）
    void EndHit () {
        //武器の判定を消す
        _weaponList.ForEach (weapon => weapon.GetComponent<EnemyWeapon> ().ActiveCollision (false));
        
        if (CurrentState == Enemy.State.DEAD) return;

        //攻撃範囲から出れば攻撃をやめる
        if ((gameObject.transform.position - _target.transform.position).sqrMagnitude > Mathf.Pow (_attackRange, 2) + ERROR_RANGE) {
            IsAction = false;
            _anim.CrossFade ("Move", 0);
            CurrentState = State.DISCOVERY;
            _comboCount = 0;
            return;
        }
        //行動終了
        IsAction = false;
    }

    void HitWakeUp () {
        if (CurrentState == Enemy.State.DEAD) return;
        //眠り判定を解除
        _isSleeping = false;
        //Change State
        CurrentState = State.DISCOVERY;
    }
}