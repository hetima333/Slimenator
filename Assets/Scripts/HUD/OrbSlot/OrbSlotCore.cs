using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// オーブスロットのコア
/// </summary>
public class OrbSlotCore : MonoBehaviour {

	// オーブスロットの大きさ
	public static readonly int SLOT_SIZE = 3;

	// スロットのリアクティブコレクション
	private ReactiveCollection<Orb> _slot = new ReactiveCollection<Orb>();

	// 予想されるスキル
	private ReactiveProperty<Skill> _expectedSkill = new ReactiveProperty<Skill>();

	/// <summary>
	/// スロットの内容
	/// </summary>
	public IReadOnlyReactiveCollection<Orb> Slot {
		get { return _slot; }
	}

	/// <summary>
	/// 現在のスロットで生成されるスキル
	/// </summary>
	public IReadOnlyReactiveProperty<Skill> ExpectedSkill {
		get { return _expectedSkill; }
	}

	// HUDManagerを一時的にシリアライズ
	[SerializeField]
	private HUDManager _manager;

	void Start() {
		var player = _manager.Player;

		// スロットの初期化
		for (int i = 0; i < SLOT_SIZE; i++) {
			_slot.Add(Orb.NONE);
		}

		// Update毎にスロット内の情報を更新
		this.UpdateAsObservable()
			.Subscribe(_ => {
				var orbs = player.GetOrbsInSlot()
					.Select(x => x.ToOrb())
					.ToArray();

				// スロット1つ1つに対して処理
				for (int i = 0; i < SLOT_SIZE; i++) {
					// オーブがスロットより少なかったら空
					_slot[i] = i >= orbs.Length ? Orb.NONE : orbs[i];
				}
			});

		// 生成されるスキルの更新
		player.ObserveEveryValueChanged(x => x.CurrentSkillOutcome())
			// 1回目はスルー
			.Skip(1)
			.DistinctUntilChanged()
			.Subscribe(x => _expectedSkill.SetValueAndForceNotify(x));
	}

}