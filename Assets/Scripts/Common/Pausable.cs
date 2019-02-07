using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	public bool PauseChildren {
		get { return _pauseChildren; }
		set {
			if (_prevPausing) {
				Debug.Assert(false, "An attempt was made to change the temporary stop target while pausing");
			}
			_pauseChildren = value;
		}
	}

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

		// ゲームを停止する
		Time.timeScale = 0.0f;

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
			GameObject[] monos = transform.GetComponentsInChildren<MonoBehaviour>().Select(x => x.gameObject).Union(_ignoreGameObjects).ToArray();
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
		// MonoBehaviourの再開
		foreach (var monoBehaviour in _pausingMonoBehaviours) {
			if(monoBehaviour == null) continue;
			monoBehaviour.enabled = true;
		}

		// ゲームを再開する
		Time.timeScale = 1.0f;
	}
}