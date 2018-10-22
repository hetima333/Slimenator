using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	// 数値が誕生してから消えるまで
	private float _lifeTime = 1.5f;

	// 移動方向
	private Vector2 _direction = Vector2.one;

	// 移動速度
	private Vector2 _velocity = Vector2.one;

	// 落下速度
	private float _gravity;

	// Update is called once per frame
	void Update () {
		transform.position +=  new Vector3(_direction.x * _velocity.x, _direction.y * _velocity.y);

		_velocity.y -= _gravity;
	}

	// アクティブになったら
	void OnEnable(){
		// X方向のランダム速度
		_velocity.x = Random.Range(0.1f, 1.0f);
		// 50%の確率で反転する
		if(Random.Range(0, 2) == 0){
			_velocity.x *= -1.0f;
		}

		// 落下速度の設定
		_gravity = _velocity.y / 20.0f;

		StartCoroutine(DestroyText());
	}

	public void SetDamage(int damage){
		GetComponent<Text>().text = damage.ToString();
	}

	IEnumerator DestroyText(){
		yield return new WaitForSeconds(_lifeTime);

		// 数値のリセット
		_velocity = Vector2.one;

		ObjectManager.Instance.ReleaseObject(this.gameObject);
	}
}