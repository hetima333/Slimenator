using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCountPresenter : MonoBehaviour {

	[SerializeField]
	private Text _healthUpgradeText;

	[SerializeField]
	private Text _skillUpgradeText;

	[SerializeField]
	private Stats _playerStats;

	// Use this for initialization
	void Start() {
		
		// HPのアップグレード段階を更新
		_playerStats.ObserveEveryValueChanged(x => x.HealthMultiplyerProperties)
			.Subscribe(x => {
				_healthUpgradeText.text = ((int) ((_playerStats.HealthMultiplyerProperties - 1) / 0.2f + 1)).ToString();
			});

		// スキルのアップグレード段階を更新
		_playerStats.ObserveEveryValueChanged(x => x.SkillPowerMultiplyerProperties)
			.Subscribe(x => {
				_skillUpgradeText.text = ((int) ((_playerStats.SkillPowerMultiplyerProperties - 1) / 0.2f + 1)).ToString();
			});
	}
}