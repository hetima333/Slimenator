using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager> {

	// プール済みのオブジェクト
	// key : objのインスタンスID, value : 該当するインスタンスIDのゲームオブジェクト
	private Dictionary<int, List<GameObject>> _pooledObjects = new Dictionary<int, List<GameObject>>();

	/// <summary>
	/// 同じ種類のオブジェクトが既にプールされているか
	/// </summary>
	/// <returns></returns>
	private bool CheckPooledObject(GameObject obj){

		// プレハブのキーを取得する
		int key = obj.GetInstanceID();
		// 同じ種類のオブジェクトがプールされているかを確認する
		bool isPooled = _pooledObjects.ContainsKey(key);

		// キーが存在しない場合は新たにリストを作成する
		if(isPooled != true){
			_pooledObjects.Add(key, new List<GameObject>());
		}
		return isPooled;
	}

	/// <summary>
	/// オブジェクトプーリングを利用してインスタンス化を行なう
	/// </summary>
	/// <returns></returns>
	public GameObject InstantiateWithObjectPooling(GameObject obj, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()){

		// 同じ種類のオブジェクトがすでにプールされているかをチェックする
		CheckPooledObject(obj);

		int key = obj.GetInstanceID();
		GameObject go = null;

		// 使用済みオブジェクトを探す
		var target = _pooledObjects[key].FirstOrDefault(x => x.activeInHierarchy != true);

		// 使用済みオブジェクトがあれば再利用する
		if(target != null){
			target.transform.position = position;
			target.transform.rotation = rotation;
			target.SetActive(true);
			go = target;
		}
		else{
			// 使用済みオブジェクトがない場合は新規に作成する
			go = Instantiate(obj);
			// 作成したオブジェクトをリストに追加する
			_pooledObjects[key].Add(go);
		}

		return target;
	}

	/// <summary>
	/// オブジェクトプールにおいて未使用状態にする
	/// </summary>
	/// <param name="obj">未使用にするGameObject</param>
	public void ReleaseObject(GameObject obj){
		obj.SetActive(false);
	}

	/// <summary>
	/// アクティブなオブジェクトのリストを返す
	/// </summary>
	/// <returns></returns>
	public List<GameObject> GetActiveObjects(GameObject obj){
		if(CheckPooledObject(obj) != true){
			return null;
		}
		return _pooledObjects[obj.GetInstanceID()].Where(x => x.activeInHierarchy == true).ToList();
	}

	/// <summary>
	/// 非アクティブなオブジェクトのリストを返す
	/// </summary>
	/// <returns></returns>
	public List<GameObject> GetSleepObjects(GameObject obj){
		if(CheckPooledObject(obj) != true){
			return null;
		}
		return _pooledObjects[obj.GetInstanceID()].Where(x => x.activeInHierarchy != true).ToList();
	}
}
