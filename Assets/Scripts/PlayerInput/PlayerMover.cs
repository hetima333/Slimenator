using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

	[SerializeField]
	private float _speed = 20.0f;

	[SerializeField]
	private Camera _camera;

	void Start () {
		var input = GetComponent<IPlayerInput> ();
		var rb = GetComponent<Rigidbody> ();
		var camera = Instantiate (_camera);
		var player = GetComponent<EntityPlayer> ();

		// 移動方向に入力を監視して移動
		input.MoveDirection
			.Where (_ => player.GetPlayerState () != EnumHolder.States.DIE)
			.Subscribe (dir => {
				rb.velocity = dir * _speed * player._Player_Stats.SpeedMultiplyerProperties;
			});

		// マウス座標を元にプレイヤーの向きを判断する
		input.MousePosition
			.Where (_ => player.GetPlayerState () != EnumHolder.States.DIE)
			.DistinctUntilChanged()
			.Subscribe (position => {
				// マウス移動量を見てその方向を向く
				var target = camera.ScreenToWorldPoint (position);
				target.y = transform.position.y;
				transform.LookAt (target);
			});

		// 移動方向用のカメラをプレイヤーに追尾
		this.UpdateAsObservable ()
			.Subscribe (_ => {
				camera.transform.position = transform.position + new Vector3 (0, 50, 0);
			});
	}
}