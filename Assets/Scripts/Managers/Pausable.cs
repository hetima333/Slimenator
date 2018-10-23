using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Rigidbodyの速度を保存しておくクラス
/// </summary>
public class RigidbodyVelocity {
	public Vector3 velocity;
	public Vector3 angularVeloccity;
	public RigidbodyVelocity(Rigidbody rigidbody) {
		velocity = rigidbody.velocity;
		angularVeloccity = rigidbody.angularVelocity;
	}
}

public class Pausable : SingletonMonoBehaviour<Pausable> {

	/// <summary>
	/// 無視するGameObject
	/// </summary>
	[SerializeField]
	private GameObject[] _ignoreGameObjects;

	/// <summary>
	/// ポーズ状態が変更された瞬間を調べるため、前回のポーズ状況を記録しておく
	/// </summary>
	private bool _prevPausing;

	/// <summary>
	/// Rigidbodyのポーズ前の速度の配列
	/// </summary>
	private RigidbodyVelocity[] _rigidbodyVelocities;

	/// <summary>
	/// ポーズ中のRigidbodyの配列
	/// </summary>
	private Rigidbody[] _pausingRigidbodies;

	/// <summary>
	/// ポーズ中のMonoBehaviourの配列
	/// </summary>
	private MonoBehaviour[] _pausingMonoBehaviours;

	/// <summary>
	/// 現在Pause中か？
	/// </summary>
	public bool Pausing {
		get;
		set;
	}

	/// <summary>
	/// 子を停止するか？
	/// </summary>
	/// <value></value>
	[SerializeField]
	private bool _pauseChildren;

	/// <summary>
	/// 更新処理
	/// </summary>
	void Update() {
		// ポーズ状態が変更されていたら、Pause/Resumeを呼び出す。
		if (_prevPausing != Pausing) {
			if (Pausing) {
				Pause();
			} else {
				Resume();
			}
			_prevPausing = Pausing;
		}
	}

	/// <summary>
	/// 中断
	/// </summary>
	void Pause() {
		// Rigidbodyの停止
		Predicate<Rigidbody> rigidbodyPredicate = null;

		// 子を停止する
		if (_pauseChildren) {
			// スリープ中でなく、IgnoreGameObjectsに含まれていないRigidbodyを抽出
			rigidbodyPredicate =
				obj => !obj.IsSleeping() &&
				Array.FindIndex(_ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
			_pausingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
		}
		// 子以外を停止する
		else {
			// 子と無視リストの重複なし配列の作成
			GameObject[] rbs = transform.GetComponentsInChildren<Rigidbody>().Select(x => x.gameObject).ToArray().Union(_ignoreGameObjects).ToArray();
			// スリープ中でなく、IgnoreGameObjectsに含まれておらず、子でもないRigidbodyを抽出
			rigidbodyPredicate =
				obj => !obj.IsSleeping() &&
				Array.FindIndex(rbs, gameObject => gameObject == obj.gameObject) < 0;
			_pausingRigidbodies = Array.FindAll(FindObjectsOfType<Rigidbody>(), rigidbodyPredicate);
		}
		// 速度配列の確保
		_rigidbodyVelocities = new RigidbodyVelocity[_pausingRigidbodies.Length];
		for (int i = 0; i < _pausingRigidbodies.Length; i++) {
			// 速度、角速度も保存しておく
			_rigidbodyVelocities[i] = new RigidbodyVelocity(_pausingRigidbodies[i]);
			_pausingRigidbodies[i].Sleep();
		}

		// MonoBehaviourの停止
		Predicate<MonoBehaviour> monoBehaviourPredicate = null;

		// 子を停止する
		if (_pauseChildren) {
			// 子要素から、有効かつこのインスタンスでないもの、IgnoreGameObjectsに含まれていないMonoBehaviourを抽出
			monoBehaviourPredicate = obj => obj.enabled &&
				obj != this &&
				Array.FindIndex(_ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;

			_pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>().Where(obj => obj.gameObject != this.gameObject).ToArray(), monoBehaviourPredicate);
		}
		// 子以外を停止する
		else {
			// 子と無視リストの重複なし配列の作成
			GameObject[] monos = transform.GetComponentsInChildren<MonoBehaviour>().Select(x => x.gameObject).ToArray().Union(_ignoreGameObjects).ToArray();

			// 子要素から、有効かつこのインスタンスでないもの、IgnoreGameObjectsに含まれていないMonoBehaviourを抽出
			monoBehaviourPredicate = obj => obj.enabled &&
				obj != this &&
				Array.FindIndex(monos, gameObject => gameObject == obj.gameObject) < 0;

			_pausingMonoBehaviours = Array.FindAll(FindObjectsOfType<MonoBehaviour>().Where(obj => obj.gameObject != this.gameObject).ToArray(), monoBehaviourPredicate);

		}

		foreach (var monoBehaviour in _pausingMonoBehaviours) {
			monoBehaviour.enabled = false;
		}
	}

	/// <summary>
	/// 再開
	/// </summary>
	void Resume() {
		// Rigidbodyの再開
		for (int i = 0; i < _pausingRigidbodies.Length; i++) {
			_pausingRigidbodies[i].WakeUp();
			_pausingRigidbodies[i].velocity = _rigidbodyVelocities[i].velocity;
			_pausingRigidbodies[i].angularVelocity = _rigidbodyVelocities[i].angularVeloccity;
		}

		// MonoBehaviourの再開
		foreach (var monoBehaviour in _pausingMonoBehaviours) {
			monoBehaviour.enabled = true;
		}
	}
}