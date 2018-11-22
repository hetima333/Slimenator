using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPress : BossSkill {

	private const float LANDING_TIME = 0.5f;

	[SerializeField]
	private GameObject _marker;

	private void Start () {
		//_marker = Resources.Load ("EnemyItem/Marker", typeof (GameObject)) as GameObject;
		_target = GameObject.Find ("Player");
		_maxCoolTime = 5;
		_actTime = Random.Range (3, 5);
		_coolTime = _maxCoolTime;
		_Type = AttackType.PHYSICAL;
	}

	override public void Action () {
		_maxCoolTime = 5;
		_coolTime = _maxCoolTime;
		_actTime = Random.Range (3, 5);
		_rid.velocity = Vector3.zero;
		if (_boss._canAnimation) {
			_boss._anim.CrossFade ("Jump", 0);
		}
		Jump ();
		_canActive = false;
		_boss.GetComponent<BossBase> ()._isAction = true;

	}

	private void Jump () {
		// 標的の座標
		Vector3 targetPosition = _target.transform.position;
		//_marker.SetActive (true);
		//_marker.transform.position = new Vector3 (targetPosition.x, 0.1f, targetPosition.z)
		JumpFixedTime (targetPosition, _actTime - LANDING_TIME);

	}

	private void JumpFixedTime (Vector3 targetPos, float time) {
		float speedVec = ComputeVectorFromTime (targetPos, time);
		float angle = ComputeAngleFromTime (targetPos, time);

		if (speedVec <= 0.0f) {
			// その位置に着地させることは不可能のようだ！
			Debug.LogWarning ("!!");
			return;
		}

		Vector3 vec = ConvertVectorToVector3 (speedVec, angle, targetPos);

		Vector3 force = vec * _rid.mass;

		_rid.AddForce (force, ForceMode.Impulse);
	}
	private Vector3 ConvertVectorToVector3 (float i_v0, float angle, Vector3 targetPos) {
		Vector3 startPosition = gameObject.transform.position;
		Vector3 targetPositon = targetPos;
		startPosition.y = 0.0f;
		startPosition.y = 0.0f;

		Vector3 dir = (targetPositon - startPosition).normalized;
		Quaternion yawRot = Quaternion.FromToRotation (Vector3.right, dir);
		Vector3 vec = i_v0 * Vector3.right;

		vec = yawRot * Quaternion.AngleAxis (angle, Vector3.forward) * vec;

		return vec;
	}

	private float ComputeVectorFromTime (Vector3 i_targetPosition, float i_time) {
		Vector2 vec = ComputeVectorXYFromTime (i_targetPosition, i_time);

		float v_x = vec.x;
		float v_y = vec.y;

		float v0Square = v_x * v_x + v_y * v_y;
		// 負数を平方根計算すると虚数になってしまう。
		// 虚数はfloatでは表現できない。
		// こういう場合はこれ以上の計算は打ち切ろう。
		if (v0Square <= 0.0f) {
			return 0.0f;
		}

		float v0 = Mathf.Sqrt (v0Square);

		return v0;
	}

	private float ComputeAngleFromTime (Vector3 i_targetPosition, float i_time) {
		Vector2 vec = ComputeVectorXYFromTime (i_targetPosition, i_time);

		float v_x = vec.x;
		float v_y = vec.y;

		float rad = Mathf.Atan2 (v_y, v_x);
		float angle = rad * Mathf.Rad2Deg;

		return angle;
	}

	private Vector2 ComputeVectorXYFromTime (Vector3 targetPos, float time) {
		// 瞬間移動は無し
		if (time <= 0.0f) {
			return Vector2.zero;
		}

		// xz平面の距離を計算。
		Vector2 startPosition = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z);
		Vector2 targetPositon = new Vector2 (targetPos.x, targetPos.z);
		float distance = Vector2.Distance (targetPositon, startPosition);

		float x = distance;
		// な、なぜ重力を反転せねばならないのだ...
		float g = -Physics.gravity.y;
		float y0 = gameObject.transform.position.y;
		float y = targetPos.y;
		float t = time;

		float v_x = x / t;
		float v_y = (y - y0) / t + (g * t) / 2;

		return new Vector2 (v_x, v_y);
	}

}