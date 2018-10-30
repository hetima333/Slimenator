using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTest : MonoBehaviour {

	[SerializeField]
	private GameObject _panelPrefab;

	private GameObject _panel;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Pausable.Instance.Pausing = !Pausable.Instance.Pausing;
			if (Pausable.Instance.Pausing) {
				_panel = Instantiate(_panelPrefab);
			} else {
				Destroy(_panel);
			}
		}
	}
}