using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	// プレイヤーへの参照
	private EntityPlayer _player;
	public EntityPlayer Player {
		get {
			if (_player == null) {
				_player = GameObject.Find("Player").GetComponent<EntityPlayer>();
			}
			return _player;
		}
	}
}