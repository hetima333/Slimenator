using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCore : MonoBehaviour {

	// Singleton
	private static ManagerCore _instance;
	public static ManagerCore Instance{
		get{
			if (_instance == null) {
				// シーン上から取得する
				_instance = FindObjectOfType<ManagerCore> ();
				if (_instance == null) {
					// ゲームオブジェクトを作成しコンポーネントを追加する
					_instance = new GameObject ("ObjectPool").AddComponent<ManagerCore>();
				}
			}
			return _instance;
		}
	}

	// Manager Properties
	private HUDManager _hud;
	public HUDManager Hud{
		get {
			if(_hud == null){
				_hud = GetComponentInChildren<HUDManager>();
			}
			return _hud;
		}
	}

	private AudioManager _audio;
	public AudioManager Audio{
		get {
			if(_audio == null){
				_audio = GetComponentInChildren<AudioManager>();
			}
			return _audio;
		}
	}

	private ObjectManager _objects;
	public ObjectManager Objects{
		get {
			if(_objects == null){
				_objects = GetComponentInChildren<ObjectManager>();
			}
			return _objects;
		}
	}
}
