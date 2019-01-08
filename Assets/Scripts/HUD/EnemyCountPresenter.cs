using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountPresenter : MonoBehaviour {

	public int EnemyCount {
		get{return GameStateManager.Instance._norm;}
		set{}
	}

	void Start() {
		var text = GetComponentInChildren<Text>();

		text.ObserveEveryValueChanged(_ => EnemyCount)
			.DistinctUntilChanged()
			.Subscribe(x => {
				text.text = "x " + x;
			});
	}
}