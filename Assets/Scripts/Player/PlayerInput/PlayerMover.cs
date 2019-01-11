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

	// 視点変更用の床
	Plane plane = new Plane();

	void Start() {
		var input = GetComponent<IPlayerInput>();
		var rb = GetComponent<Rigidbody>();
		var camera = Instantiate(_camera);
		var player = GetComponent<EntityPlayer>();

		// 移動方向に入力を監視して移動
		input.MoveDirection
			.Where(_ => player.GetPlayerState() != EnumHolder.States.DIE)
			// .Where(_ => player.Controllable)
			.Subscribe(dir => {
				if (player.Controllable) {
					rb.velocity = dir * _speed * player._Player_Stats.SpeedMultiplyerProperties;
				} else {
					rb.velocity = Vector3.zero;
				}
			});

		// マウス座標を元にプレイヤーの向きを判断する
		input.MousePosition
			.Where(_ => player.GetPlayerState() != EnumHolder.States.DIE)
			.DistinctUntilChanged()
			.Subscribe(position => {
				// カメラとマウスの位置を元にRayを準備
				var ray = Camera.main.ScreenPointToRay(position);
				float distance = 0;

				// プレイヤーの高さにPlaneを更新して、カメラの情報を元に地面判定して距離を取得
				plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
				if (plane.Raycast(ray, out distance)) {

					// 距離を元に交点を算出して、交点の方を向く
					var lookPoint = ray.GetPoint(distance);
					transform.LookAt(lookPoint);
				}
			});

		// 移動方向用のカメラをプレイヤーに追尾
		this.UpdateAsObservable()
			.Subscribe(_ => {
				camera.transform.position = transform.position + new Vector3(0, 50, 0);
			});
	}
}