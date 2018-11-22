using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// PC版プレイヤー入力
/// </summary>
public class PlayerInput : MonoBehaviour, IPlayerInput {

	private Subject<Vector3> moveDirectionSubject = new Subject<Vector3>();
	public ReadOnlyReactiveProperty<Vector3> MoveDirection {
		get { return moveDirectionSubject.ToReadOnlyReactiveProperty(); }
	}

	private Subject<float> scrollDirectionSubject = new Subject<float>();
	public ReadOnlyReactiveProperty<float> ScrollDirection {
		get { return scrollDirectionSubject.ToReadOnlyReactiveProperty(); }
	}

	private Subject<bool> useSkillButtonSubject = new Subject<bool>();
	public ReadOnlyReactiveProperty<bool> UseSkillButton {
		get { return useSkillButtonSubject.ToReadOnlyReactiveProperty(); }
	}

	private Subject<bool> combineOrbButtonSubject = new Subject<bool>();
	public ReadOnlyReactiveProperty<bool> CombineOrbButton {
		get { return combineOrbButtonSubject.ToReadOnlyReactiveProperty(); }
	}

	private Subject<bool> suckingButtonSubject = new Subject<bool>();
	public IObservable<bool> SuckingButton {
		get { return suckingButtonSubject.AsObservable(); }
	}

	void Start() {
		// 移動入力
		this.UpdateAsObservable()
			.Select(_ => (new Vector3(
				Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")
			).normalized))
			.Subscribe(moveDirectionSubject);

		// スクロール入力
		this.UpdateAsObservable()
			.Select(_ => Input.GetAxis("Mouse ScrollWheel"))
			.Subscribe(scrollDirectionSubject);

		// スキル使用ボタン
		this.UpdateAsObservable()
			.Select(_ => Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
			.Subscribe(useSkillButtonSubject);

		// オーブ合成ボタン
		this.UpdateAsObservable()
			.Select(_ => Input.GetKey(KeyCode.LeftShift))
			.Subscribe(combineOrbButtonSubject);

		// 吸い込みボタン
		this.UpdateAsObservable()
			.Select(_ => Input.GetKey(KeyCode.Mouse0))
			.Subscribe(suckingButtonSubject);
	}
}