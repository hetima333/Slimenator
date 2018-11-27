/// 索敵用スフィアの処理
/// Processing of Spear of Searching Spots
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/31
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPlayer : MonoBehaviour {

	private Enemy _enemy;

	private SphereCollider _col;

	// Use this for initialization
	void Awake () {
		_enemy = GetComponentInParent<Enemy> ();
	}

	public void Initialize () {
		//索敵用コライダーの設定
		_col = GetComponent<SphereCollider> ();
		//TriggerOn
		_col.isTrigger = true;
		//範囲設定
		_col.radius = _enemy._searchRange;

	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			_enemy.Discover (col.gameObject);
		}
	}

	//戦闘範囲離脱時の処理
	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player") {
			if (_enemy.CurrentState != Enemy.State.DEAD) {
				//Set Target
				_enemy._target = null;

				_enemy.IsAction = false;
				//Change State
				_enemy.CurrentState = Enemy.State.FREE;
			}
		}
	}

}