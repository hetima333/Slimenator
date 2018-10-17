using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager> {

	// プール済みのオブジェクト
	// key : prefabのインスタンスID, value : 該当するインスタンスIDのゲームオブジェクト
	private Dictionary<int, List<GameObject>> _pooledObjects = new Dictionary<int, List<GameObject>>();

	/// <summary>
	/// 同じ種類のオブジェクトが既にプールされているか
	/// </summary>
	/// <returns></returns>
	private bool CheckPooledObject(GameObject prefab){

		// プレハブのキーを取得する
		int key = prefab.GetInstanceID();
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
	public GameObject InstantiateWithObjectPooling(GameObject prefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()){

		// 同じ種類のオブジェクトがすでにプールされているかをチェックする
		CheckPooledObject(prefab);

		int key = prefab.GetInstanceID();
		GameObject obj = null;

		// 使用済みオブジェクトを探す
		var target = _pooledObjects[key].FirstOrDefault(x => x.activeInHierarchy != true);

		// 使用済みオブジェクトがあれば再利用する
		if(target != null){
			target.transform.position = position;
			target.transform.rotation = rotation;
			target.SetActive(true);
			obj = target;
		}
		else{
			// 使用済みオブジェクトがない場合は新規に作成する
			obj = Instantiate(prefab);
			// 作成したオブジェクトをリストに追加する
			_pooledObjects[key].Add(obj);
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
}
