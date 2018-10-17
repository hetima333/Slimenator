/// <summary>
/// プレイヤーのステータスインターフェース
/// </summary>

public interface IPlayerStats {
	// HPのインターフェース
	int HitPoint{ get; }

	// 最大HPのインターフェース
	int MaxHitPoint{ get; }
}
