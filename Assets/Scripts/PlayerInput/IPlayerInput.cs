using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーの入力インタフェース
/// </summary>
public interface IPlayerInput {
	/// <summary>
	/// プレイヤーの移動方向
	/// </summary>
	ReadOnlyReactiveProperty<Vector3> MoveDirection { get; }

	/// <summary>
	/// スクロール方向
	/// </summary>
	ReadOnlyReactiveProperty<float> ScrollDirection { get; }

	/// <summary>
	/// スキル使用ボタン
	/// </summary>
	/// <value></value>
	ReadOnlyReactiveProperty<bool> UseSkillButton { get; }

	/// <summary>
	/// オーブを合成するボタン
	/// </summary>
	ReadOnlyReactiveProperty<bool> CombineOrbButton { get; }

	/// <summary>
	/// 吸い込みボタンを押しているか？
	/// </summary>
	IObservable<bool> SuckingButton { get; }

	/// <summary>
	/// マウスポジション
	/// </summary>
	IObservable<Vector3> MousePosition { get; }
}