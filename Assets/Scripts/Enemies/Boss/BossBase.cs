using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SimpleAnimation))]
public class BossBase : MonoBehaviour, IDamageable {

	public enum State {
		ALIVE,
		DEAD
	}

	public State _state = State.ALIVE;

	protected const float ACT_INTERVAL = 1.0f;
	protected float _actInterval = ACT_INTERVAL;
	public float _maxHp;
	public float _hp;
	//インタフェース用最大Hp取得
	public float MaxHitPoint { get { return _maxHp; } }
	//インタフェース用現在Hp取得
	public float HitPoint { get { return _hp; } }

	[SerializeField]
	public bool _isGround = true;

	public bool IsGround { get { return _isGround; } }

	public GameObject _target;

	public bool _isAction = false;

	protected int _phase = 0;

	//アニメーション
	public SimpleAnimation _anim;

	//Object for long range attack
	public GameObject _slimeBullet;

	public BossSkill _previousSkill;

	[SerializeField]
	//発動できるスキルリスト
	public List<BossSkill> _skillList = new List<BossSkill> ();

	public List<BossSkill> _canUseSkillList = new List<BossSkill> ();

	[SerializeField]
	public bool _canAnimation = true;

	public void TakeDamage (float damage) {
		_hp -= damage;
	}

	private void OnCollisionEnter (Collision col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && _state == BossBase.State.ALIVE) {
			if (_isGround == false) {
				Debug.Log ("着地");
				if (_canAnimation) {
					_anim.CrossFade ("Fall", 0);
				}
				_isGround = true;
			}
		}
	}

	private void OnCollisionExit (Collision col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && _state == BossBase.State.ALIVE) {
			_isGround = false;
		}
	}

}