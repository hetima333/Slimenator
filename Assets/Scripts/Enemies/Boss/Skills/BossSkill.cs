using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour {

	public float _maxCoolTime = 3.0f;
	public float _coolTime;
	public bool _canActive = true;
	public bool _isActive = false;

	public GameObject _target;

	public GameObject _boss;

	private void Awake () {
		_boss = this.gameObject;

		_coolTime = _maxCoolTime;

		_target = _boss.GetComponent<TestBoss> ()._target;

	}

	void Update () {
		if (!_isActive && !_canActive) {

			_coolTime -= Time.deltaTime;
			if (_coolTime <= 0) {
				_coolTime = _maxCoolTime;
				_canActive = true;
				ActEnd ();
			}
		}
	}

	public virtual void Action () { }

	void ActEnd () {
		_isActive = false;
		_boss.GetComponent<TestBoss> ()._isAction = false;

	}
}