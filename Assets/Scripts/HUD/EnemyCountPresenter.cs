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
				if(x <=0 ){x = 0;}
				text.text = "x " + x;
			});
	}
}