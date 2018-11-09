using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// スキルスロットのコア
/// </summary>
public class SkillSlotCore : MonoBehaviour {

	// スキルスロットの大きさ
	public static readonly int SLOT_SIZE = 3;

	// 選択中スキル番号
	private IntReactiveProperty _selectedSkillNumber = new IntReactiveProperty();

	// スキルスロットのリアクティブプロパティ
	private ReactiveCollection<Skill> _slot = new ReactiveCollection<Skill>();

	/// <summary>
	/// 選択中スキル番号
	/// </summary>
	public IntReactiveProperty SelectedSkillNumber {
		get { return _selectedSkillNumber; }
	}

	public IReadOnlyReactiveCollection<Skill> Slot {
		get { return _slot; }
	}

	[SerializeField]
	private HUDManager _manager;

	void Start() {
		var player = _manager.Player;

		// 選択中スキル番号の更新
		player.ObserveEveryValueChanged(x => x.CurrentSelectedSkill())
			.Subscribe(x => _selectedSkillNumber.SetValueAndForceNotify(x));

		// スロットの初期化
		for (int i = 0; i < SLOT_SIZE; i++) {
			_slot.Add(null);
		}

		// Update毎にスキルスロット内の情報を更新する
		this.UpdateAsObservable()
			.Subscribe(_ => {
				var skills = player.GetSkillsInSlot();

				for (int i = 0; i < SLOT_SIZE; i++) {
					_slot[i] = i >= skills.Count ? null : skills[i];
				}
			});
	}
}