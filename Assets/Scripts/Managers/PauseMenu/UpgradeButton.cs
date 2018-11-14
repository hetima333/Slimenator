using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MenuButtonBase {

	[SerializeField]
	private Stats _playerStats;

	private enum Upgrade {
		SPEED,
		SUCK,
		HEALTH
	}

	[SerializeField]
	private Upgrade _upgrade;

	protected override void OnExecute(PointerEventData e) {
		switch (_upgrade) {
			case Upgrade.SPEED:
				_playerStats.SpeedMultiplyerProperties++;
				break;
			case Upgrade.SUCK:
				_playerStats.SuckingPowerMultiplyerProperties++;
				break;
			case Upgrade.HEALTH:
				_playerStats.HealthMultiplyerProperties++;
				break;
		}
	}
}
