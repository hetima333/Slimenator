/// ダメージのインターフェース
/// Interface of Damage
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/11

public interface IDamageable
{
    // MaxHP
    float MaxHitPoint { get; }

	// HP
	float HipPoint{ get; }

	//ダメージの適用
	void TakeDamage(float Damage);
}