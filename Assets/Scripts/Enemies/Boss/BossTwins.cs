﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwins : BossBase, IDamageable {

	//TODO Boss Performance
	const float MAX_HP = 1000.0f;
	const float MONEY = 2000.0f;
	public GameObject _avatar;

	public bool _isAlone = false;

	private float graceTime = 5f;

	[SerializeField]
	private bool _isGround;

	public bool IsGround { get { return _isGround; } }

	// Use this for initialization
	void Start () {
		SetStatus ();
		PhaseUp ();
		_target = GameObject.Find ("Player");

	}

	// Update is called once per frame
	void Update () {

		if (gameObject.transform.position.y > gameObject.transform.localScale.y + 1.5f) {
			_isGround = false;
		} else {
			_isGround = true;
		}

		_actInterval -= Time.deltaTime; {
			if (_actInterval <= 0) {
				_actInterval = ACT_INTERVAL;
				UseSkill ();
			}
		}

		//１人になった場合
		if (_isAlone) {
			graceTime -= Time.deltaTime;
			if (graceTime <= 0) {
				PhaseUp ();
				_isAlone = false;
			}
		}

	}

	private void SetStatus () {
		_maxHp = MAX_HP;
		_hp = _maxHp;
	}

	//ダメージを受ける
	public void TakeDamage (float damage) {
		_hp -= damage;

		if (_hp <= 0) {

		}
	}

	private void UseSkill () {

		if (_isAction) { Debug.Log ("今忙しい"); return; }

		_canUseSkillList.Clear ();

		foreach (var skill in _skillList) {
			if (skill._canActive == true) {
				_canUseSkillList.Add (skill);
			}
		}

		if (_canUseSkillList.Count == 0) { Debug.Log ("今使えるの無い"); return; }

		var pinBallComponent = GetComponent<PinballAttack> ();

		//ピンボール攻撃が出来るかつ相手が設置状態
		if (pinBallComponent._canActive == true && _avatar.GetComponent<BossTwins> ().IsGround == true) {
			if (_isGround && _previousSkill != pinBallComponent) {
				pinBallComponent.Action ();
				_avatar.GetComponent<PinballAttack> ().Action ();
				_previousSkill = pinBallComponent;
				_avatar.GetComponent<BossTwins> ()._previousSkill = _avatar.GetComponent<PinballAttack> ();
				return;
			}
		}

		Vector3 lookPos = _target.transform.position;

		lookPos.y = gameObject.transform.position.y;

		transform.LookAt (lookPos);

		//ランダムなスキルを呼び出す。
		if (_canUseSkillList.Count != 0) {
			var skill = _canUseSkillList[Random.Range (0, _canUseSkillList.Count)];
			//一回前に使ったスキルではない。かつ発動できる場合実行
			if (skill != _previousSkill) {
				skill.Action ();
				_previousSkill = skill;
				_canUseSkillList.Remove (skill);

			} else {
				Debug.Log ("被っとるんじゃ");
			}
		}

	}

	private void PhaseUp () {
		_phase++;
		Debug.Log ("Phase" + _phase);

		switch (_phase) {
			case 1:
				//新しいコンポーネントの追加
				//_skillList.Add (gameObject.AddComponent<JumpPress> ());
				_skillList.Add (gameObject.AddComponent<FrontSlimeShot> ());
				_skillList.Add (gameObject.AddComponent<AroundSlimeShot> ());
				//_skillList.Add (gameObject.AddComponent<Tackle> ());
				_skillList.Add (gameObject.AddComponent<PinballAttack> ());
				break;

			case 2:
				//メテオの追加
				break;
			default:

				break;
		}

	}

	public void SetAvatar (GameObject avatar) {
		_avatar = avatar;
	}

	public void DeadCall () {
		if (_avatar != null)
			_avatar.GetComponent<BossTwins> ()._isAlone = true;
	}

}