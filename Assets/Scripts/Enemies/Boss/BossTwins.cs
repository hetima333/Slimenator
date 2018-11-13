using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwins : BossBase, IDamageable {

	//TODO Boss Performance
	const float MAX_HP = 1000.0f;
	const float MONEY = 2000.0f;
	private GameObject _avatar;

	public bool _isAlone = false;

	private float graceTime = 5f;

	// Use this for initialization
	void Start () {
		SetStatus ();
		PhaseUp ();
		_target = GameObject.Find ("Player");

	}

	// Update is called once per frame
	void Update () {

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

		foreach (var skill in _skillList) {
			if (skill._canActive == true) {
				_canUseSkillList.Add (skill);
			}
		}

		if (_canUseSkillList.Count == 0) { Debug.Log ("今使えるの無い"); return; }

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

		_canUseSkillList.Clear ();
	}

	private void PhaseUp () {
		_phase++;
		Debug.Log ("Phase" + _phase);

		switch (_phase) {
			case 1:
				//新しいコンポーネントの追加
				_skillList.Add (gameObject.AddComponent<JumpPress> ());
				_skillList.Add (gameObject.AddComponent<FrontSlimeShot> ());
				_skillList.Add (gameObject.AddComponent<AroundSlimeShot> ());
				_skillList.Add (gameObject.AddComponent<Tackle> ());
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