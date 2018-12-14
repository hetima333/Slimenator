using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダメージ関連のUIを生成する
/// </summary>
// [RequireComponent(typeof(IDamageable))]
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

	// 追従するターゲット
	private IDamageable _target;

	// ダメージポップアップを利用するか？
	[SerializeField]
	private bool _useDamagePop = true;

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
		// ターゲットが生存していら追従する
		if (_target.HitPoint > 0) {
			// 追従する
			_canvas.transform.position = this.transform.position + _offset;
			_canvas.transform.rotation = Quaternion.Euler(Vector3.right * Camera.main.transform.rotation.x * 100);
		}
	}

	void OnEnable() {
		// 有効化された時に初期化を行なう
		Init();
	}

	private void Init() {
		if (_canvas == null) {
			return;
		}

		// HPが設定されるまで待機する
		StartCoroutine(WaitForSetHitPoint());
	}

	IEnumerator WaitForSetHitPoint() {
		// HPが設定されるまで待機
		while (GetComponent<IDamageable>().HitPoint <= 0) {
			yield return new WaitForEndOfFrame();
		}

		_target = GetComponent<IDamageable>();

		// 各パラメータの初期化
		var hpbar = _canvas.GetComponentInChildren<HPBarCore>();
		hpbar.Init(_target);
		hpbar.UseDamagePop = _useDamagePop;
	}
}