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
	private GameObject _hpbarCanvas;

	[SerializeField]
	private Vector3 _offset;

	private Canvas _canvas;

	// まとめて保管する親オブジェクト
	private GameObject _parent;

	// 親オブジェクトの名前
	private string _parentName = "UIs";

	// Use this for initialization
	void Start() {
		// 親オブジェクトが既に存在するか探索する
		_parent = GameObject.Find(_parentName);
		// 存在しなかったら
		if (_parent == null) {
			// 親オブジェクトの生成
			_parent = Instantiate(new GameObject(_parentName), Vector3.zero, Quaternion.Euler(Vector3.zero));
			// 生成タイミングが被った時に被ったほうを破棄して再設定
			if (_parent.name != _parentName) {
				Destroy(_parent);
				_parent = GameObject.Find(_parentName);
			}
		}

		var obj = Instantiate(_hpbarCanvas, _parent.transform);
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