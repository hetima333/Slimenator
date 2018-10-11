using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCore : SingletonMonoBehaviour<ManagerCore> {
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
