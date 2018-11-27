using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager> {

	// プール済みのオブジェクト
	// key : objのインスタンスID, value : 該当するインスタンスIDのゲームオブジェクト
	private Dictionary<int, List<GameObject>> _pooledObjects = new Dictionary<int, List<GameObject>>();

	// インスタンス化イベントのサブジェクト
	private Subject<GameObject> _onInstantiate = new Subject<GameObject>();

	// インスタンス化イベントの購読
	public IObservable<GameObject> OnInstantiateObservable {
		get { return _onInstantiate.AsObservable(); }
	}

	/// <summary>
	/// 同じ種類のオブジェクトが既にプールされているか
	/// </summary>
	/// <returns></returns>
	private bool CheckPooledObject(GameObject obj) {
#if UNITY_EDITOR
		// 引数のオブジェクトがプレハブでなければエラーを返す
		if (PrefabUtility.GetPrefabParent(obj) == null) {
			Debug.LogWarning("CheckPooledObject() argment must be prefab / 引数はプレハブである必要があります");
			return false;
		}
#endif
		// プレハブのキーを取得する
		int key = obj.GetInstanceID();

		// 同じ種類のオブジェクトがプールされているかを返す
		return _pooledObjects.ContainsKey(key);
	}

	/// <summary>
	/// オブジェクトプーリングを利用してインスタンス化を行なう
	/// </summary>
	/// <returns></returns>
	public GameObject InstantiateWithObjectPooling(GameObject obj, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()) {
#if UNITY_EDITOR
		// 引数のオブジェクトがプレハブでなければエラーを返す
		if (PrefabUtility.GetPrefabParent(obj) == null) {
			Debug.LogWarning("InstantiateWithObjectPooling() argment must be prefab / 引数はプレハブである必要があります");
			return null;
		}
#endif
		// インスタンスIDを取得
		int key = obj.GetInstanceID();
		GameObject go = null;

		// キーが存在しない場合は新たにリストを作成する
		if (CheckPooledObject(obj) != true) {
			_pooledObjects.Add(key, new List<GameObject>());
		}

		// 使用済みオブジェクトを探す
		var target = _pooledObjects[key].FirstOrDefault(x => x.activeInHierarchy != true);

		// 使用済みオブジェクトがあれば再利用する
		if (target != null) {
			target.transform.position = position;
			target.transform.rotation = rotation;
			target.SetActive(true);
			go = target;
		} else {
			// 使用済みオブジェクトがない場合は新規に作成する
			go = Instantiate(obj, position, rotation);
			// 作成したオブジェクトをリストに追加する
			_pooledObjects[key].Add(go);
		}

		// インスタンス化イベントの発行
		_onInstantiate.OnNext(go);

		return go;
	}

	/// <summary>
	/// オブジェクトプールにおいて未使用状態にする
	/// </summary>
	/// <param name="obj">未使用にするGameObject</param>
	public void ReleaseObject(GameObject obj) {
		obj.SetActive(false);
	}

	/// <summary>
	/// アクティブなオブジェクトのリストを返す
	/// </summary>
	/// <returns></returns>
	public List<GameObject> GetActiveObjects(GameObject obj) {
#if UNITY_EDITOR
		// 引数のオブジェクトがプレハブでなければエラーを返す
		if (PrefabUtility.GetPrefabParent(obj) == null) {
			Debug.LogWarning("GetActiveObjects() argment must be prefab / 引数はプレハブである必要があります");
			return null;
		}
#endif

		// プールされているオブジェクトが存在しなければnullを返す
		if (CheckPooledObject(obj) != true) {
			return null;
		}
		return _pooledObjects[obj.GetInstanceID()].Where(x => x.activeInHierarchy == true).ToList();
	}

	/// <summary>
	/// 非アクティブなオブジェクトのリストを返す
	/// </summary>
	/// <returns></returns>
	public List<GameObject> GetSleepObjects(GameObject obj) {

#if UNTIY_EDITOR
		// 引数のオブジェクトがプレハブでなければエラーを返す
		if (PrefabUtility.GetPrefabParent(obj) == null) {
			Debug.LogWarning("GetSleepObjects() argment must be prefab / 引数はプレハブである必要があります");
			return null;
		}
#endif

		// プールされているオブジェクトが存在しなければnullを返す
		if (CheckPooledObject(obj) != true) {
			return null;
		}
		return _pooledObjects[obj.GetInstanceID()].Where(x => x.activeInHierarchy != true).ToList();
	}
}