using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCore : MonoBehaviour {

	public void Resume() {
		Pausable.Instance.Pausing = false;
		Destroy(this.gameObject);
	}
}