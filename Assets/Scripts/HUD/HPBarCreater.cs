using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダメージ関連のUIを生成する
/// </summary>
[RequireComponent(typeof(IDamageable))]
public class HPBarCreater : MonoBehaviour {

	[SerializeField]
	private GameObject _prefab;

	[SerializeField]
	private Vector3 _offset;

	private Canvas _canvas;

	// Use this for initialization
	void Start() {
		var obj = Instantiate(_prefab);
		obj.transform.position = this.transform.position + _offset;

		_canvas = obj.GetComponent<Canvas>();
		Init();
	}

	// Update is called once per frame
	void Update() {
		// 追従する
		_canvas.transform.position = this.transform.position + _offset;
		_canvas.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	void OnEnable() {
		Init();
	}

	private void Init() {
		if (_canvas == null) {
			return;
		}
		// 各パラメータの初期化
		_canvas.GetComponentInChildren<HPBarCore>().Init(GetComponent<IDamageable>());
	}
}