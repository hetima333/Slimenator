using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballAttack : BossSkill {
	//相方のminiKingSlime
	private GameObject _avatar;

	private PhysicMaterial _physicMat;

	private float _AttackDistance = 5;

	private void Start () {
		_physicMat = (PhysicMaterial) Resources.Load ("Physics/EnemyBouns");
		_target = GameObject.Find ("Player");
		_maxCoolTime = 8.0f;
		_coolTime = _maxCoolTime;
		_avatar = gameObject.GetComponent<BossTwins> ()._avatar;
	}

	override public void Action () {
		if (!_avatar) return;
		//二体の距離を取得
		var _distance = (gameObject.transform.position - _avatar.transform.position).sqrMagnitude;
		//二体の距離が近すぎる場合はcancel
		if (_distance < Mathf.Pow (_AttackDistance, 2)) {
			Debug.Log ("ちけぇ");
			_coolTime = _maxCoolTime;
			_canActive = false;;
			return;
		}

		Attack ();

		_coolTime = _maxCoolTime;
		_actTime = 4.0f;
		_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;

		
	}

	public void Attack () {
		_rid.velocity = Vector3.zero;
		//フィジックスマテリアルの有効化
		_col.material = _physicMat;
		//相方の方を向く
		Vector3 lookPos = _avatar.transform.position;
		lookPos.y = gameObject.transform.position.y;
		transform.LookAt (lookPos);
		//軸をずらす（ピンボール感を出すため）
		transform.Rotate (new Vector3 (0, 3, 0));
		//相手に向かって体当たりを開始。
		_rid.AddForce (transform.forward * 100000);
	}

}