using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	// 数値が誕生してから消えるまで
	private float _lifeTime = 1.5f;

	// 移動方向
	private Vector2 _direction = Vector2.one;

	// 初期移動速度
	private Vector2 _defaultVelocity = new Vector2(0.1f, 0.1f);

	// 移動速度
	private Vector2 _velocity;

	// 最小移動速度
	private float _minVelocity = 0.01f;

	// 最大移動速度
	private float _maxVelocity = 0.1f;

	// 落下速度
	private float _gravity;

	// Update is called once per frame
	void Update() {
		transform.position += new Vector3(_direction.x * _velocity.x, _direction.y * _velocity.y);

		_velocity.y -= _gravity;
	}

	// アクティブになったら
	void OnEnable() {
		_velocity = _defaultVelocity;
		// X方向のランダム速度
		_velocity.x = Random.Range(_minVelocity, _maxVelocity);
		// 50%の確率で反転する
		if (Random.Range(0, 2) == 0) {
			_velocity.x *= -1.0f;
		}

		// 落下速度の設定
		_gravity = _velocity.y / 20.0f;

		StartCoroutine(DestroyText());
	}

	public void SetDamage(int damage) {
		GetComponent<Text>().text = damage.ToString();
	}

	IEnumerator DestroyText() {
		yield return new WaitForSeconds(_lifeTime);

		// 数値のリセット
		_velocity = _defaultVelocity;

		ObjectManager.Instance.ReleaseObject(this.gameObject);
	}
}