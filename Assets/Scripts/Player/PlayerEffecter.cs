using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(EntityPlayer))]
public class PlayerEffecter : MonoBehaviour {

	private bool _flashingColor = false;

	[SerializeField, Range(0.0f, 1.0f)]
	private float _flashTime = 0.1f;

	// Use this for initialization
	void Start() {
		var player = GetComponent<EntityPlayer>();
		var renderer = GetComponentInChildren<SkinnedMeshRenderer>();

		// プレイヤーのHPが減少したら
		player.ObserveEveryValueChanged(x => x.HitPoint)
			.Buffer(2, 1)
			.Where(x => x.Count == 2)
			.Select(x => x.First() - x.Last())
			.Where(x => x > 0)
			.Subscribe(x => {
				if (_flashingColor) {
					StopCoroutine(FlashColor());
				}
				StartCoroutine(FlashColor());
			});
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// 被ダメ時に点滅する
	/// </summary>
	/// <returns></returns>
	IEnumerator FlashColor() {
		_flashingColor = true;
		var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		renderer.material.color = Color.red;
		yield return new WaitForSeconds(_flashTime);
		renderer.material.color = Color.white;
		_flashingColor = false;
	}
}