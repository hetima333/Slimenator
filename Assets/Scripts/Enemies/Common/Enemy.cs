/// 敵の基本クラス
/// Base class of enemies
/// Athor： Yuhei Mastumura
/// Last edit date：2018/11/15
/// ★印は留学生に改変された部分
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必須コンポーネントの指定
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SimpleAnimation))]
[RequireComponent (typeof (EnemyMove))]

public abstract class Enemy : MonoBehaviour, IDamageable, ISuckable {

    //種類
    public enum Type { MEEL, RANGE, TANK }
    private Type _enemyType;
    public Type EnemyType { get { return _enemyType; } set { _enemyType = value; } }

    //状態一覧(待機、自由移動、発見、戻り、攻撃、死亡)
    public enum State { IDLE, FREE, DISCOVERY, RETURN, ATTACK, DEAD }

    //今の状態
    [SerializeField]
    private State _currentState;
    public State CurrentState { get { return _currentState; } set { _currentState = value; } }

    //移動速度
    public float Speed {
        get {
            return _properties.SpeedProperties * _properties.SpeedMultiplyerProperties;
        }
    }
    //索敵範囲
    public float _searchRange;
    //攻撃範囲
    public float _attackRange;
    //自由移動の幅
    public float _freeMoveRange;
    //所持金
    [SerializeField]
    private float _money;
    public float Money { get { return _money; } set { _money = value; } }
    //自由移動の目標座標
    public Vector3 _freeMovePosition;
    //初期座標
    public Vector3 _startPosition;
    //ダメージを受けているか？（ダメージモーション中か）
    private bool _isDamaged = false;
    public bool IsDamaged { get { return _isDamaged; } set { _isDamaged = value; } }
    //忍耐値（OVERでのけぞる）
    public float _patienceValue = 10;

    //行動中か？
    [SerializeField]
    private bool _isAction;
    public bool IsAction { get { return _isAction; } set { _isAction = value; } }

    [SerializeField]
    private bool _isGround = false;
    public bool IsGround { get { return _isGround; } set { _isGround = value; } }

    //移動用リジットボディ
    private Rigidbody _rigidbody;
    public Rigidbody RigidbodyProperties { get { return _rigidbody; } set { _rigidbody = value; } }
    //索敵用ゲームオブジェクト
    public GameObject _searchObj;
    //狙う対象
    public GameObject _target;
    //シンプルアニメーション
    public SimpleAnimation _anim;
    //現在再生中のアニメの名前
    public string _animName;
    //停止時のアニメの再生時間
    public float _animLastTime = 0;
    protected Status _status;
    protected Stats _properties;

    public GameObject _body;

    public Material _deadMat;

    //最大値
    public float MaxHitPoint { get { return _properties.MaxHealthProperties * _properties.HealthMultiplyerProperties; } }
    //体力
    public float HitPoint { get { return _properties.HealthProperties; } }

    public bool _isLady = false;

    public abstract void Init (Stats stat);

    //ステータスのセット関数
    public void SetStatus (Enemy.Type type, float maxHp, float speed, float searchRange, float attackRange, float moveRange, float money) {
        _body = transform.Find ("Body").gameObject;
        _deadMat = (Material) Resources.Load ("Material/Dead");
        //タイプセット
        _enemyType = type;
        //初期はアイドル
        _currentState = State.IDLE;
        //体力
        _properties.HealthProperties = MaxHitPoint;
        //索敵範囲
        if (_searchRange == 0) { _searchRange = searchRange; }
        //攻撃範囲
        if (_attackRange == 0) { _attackRange = attackRange; }
        //自由移動の幅
        if (_freeMoveRange == 0) { _freeMoveRange = moveRange; }
        //所持金の設定
        if (_money == 0) { _money = money; }
        //初期位置の記憶
        _startPosition = gameObject.transform.position;
        //Managerに生まれたっていう
        GameStateManager.Instance.IncreaseEnemy ();
        _status = gameObject.GetComponent<Status> ();
        _status.Init ();
    }

	void OnDisable()
    {
        //Debug.Log(_animName);
		//再生中のアニメーションの再生位置を記憶
		_animLastTime = _anim.GetState(_animName).time - (int)_anim.GetState(_animName).time;
        //Debug.Log(_animLastTime);
    }

    void OnEnable()
    {
		if(!_isLady)return;
		//再生中のアニメーションがあれば
		if(_animName != null)
		{
			//最後のアニメーションの再生位置を記憶しておいたものに変更
			_anim.GetState (_animName).normalizedTime = _animLastTime;
			//最後のアニメーション再生
			_anim.CrossFade(_animName,0);
		}
    }



    //ダメージを受ける
    public void TakeDamage (float damage) {
        if (_currentState == State.DEAD) return;

        if (damage > 0) {
            //ダメージ受ける
            _properties.HealthProperties -= damage;
            //TODO被ダメが許容値を超えた場合
            if (damage > _patienceValue) {
                //被ダメアニメーション
                _anim.CrossFade ("Damage", 0);
                _animName = "Damage";
                //ダメージ判定中
                IsDamaged = true;
            }
            //HPが0になる=死んだとき
            if (_properties.HealthProperties <= 0) {
                _currentState = State.DEAD;
                //死んだときの処理
                Dying ();
            }
        }
    }

    //★アニメーションを状態異常に適応させるためのLateUpdate
    protected virtual void LateUpdate () {
        
        //セットされている全てのアニメーションの速度調整（-1はデフォルトクリップ回避）
        for (int i =0 ;i< _anim.GetClipCount()-1;i++)
        {
            if (_anim.GetState (_anim.m_States[i].name).speed != _properties.SpeedMultiplyerProperties) {
                    //再生速度の変更を行う
                    _anim.GetState (_anim.m_States[i].name).speed = _properties.SpeedMultiplyerProperties;
            }
        }
    }

    //死亡アクション
    private void Dying () {
        _anim.CrossFade ("Dead", 0);
        _animName = "Dead";

    }

    //死亡したときに呼ばれる関数（AnimationEvent）
    public void Dead () {
        //死亡コルーチン
        _body.GetComponent<Renderer>().material = _deadMat;
        _body.GetComponent<DissolveTest>().enabled = true;
        StartCoroutine(DeadCol());
    }

    public IEnumerator DeadCol()
    {
        //適当な時間経過後死ぬ
        yield return new WaitForSeconds(2.0f);
        //マネージャーに死んだよっていう
        GameStateManager.Instance.DecreaseEnemy ();
        ObjectManager.Instance.ReleaseObject (gameObject);
    }

    //発見時に呼ばれる関数
    public virtual void Discover (GameObject obj) { }

    //★吸い込まれた時呼ばれる関数
    public void Sacking () {
        return;
    }
    //ダメージ判定終了（AnimationEvent用）
    void EndDamage () {
        IsDamaged = false;
    }

    private void OnCollisionEnter (Collision col) {
        if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && CurrentState != State.DEAD) {
            if (_isGround == false) {
                _isGround = true;
            }
        }
    }

    private void OnCollisionExit (Collision col) {
        if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && CurrentState != State.DEAD) {
            _isGround = false;
        }
    }
}