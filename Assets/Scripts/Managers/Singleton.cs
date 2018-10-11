using UnityEngine;
 
public class Singleton : MonoBehaviour {
 
	// インスタンスが存在するか？
	static bool existsInstance = false;
 
	// 初期化
	void Awake () {
		// インスタンスが存在するなら破棄する
		if (existsInstance)
		{
			Destroy(gameObject);
			return;
		}
 
		// 存在しない場合
		// 自身が唯一のインスタンスとなる
		existsInstance = true;
		DontDestroyOnLoad(gameObject);
	}
}