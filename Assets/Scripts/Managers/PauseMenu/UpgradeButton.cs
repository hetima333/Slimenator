using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UpgradeButton : MenuButtonBase {

	[SerializeField]
	private Stats _playerStats;

	[SerializeField]
	private HUDManager _manager;

	private enum Upgrade {
		HEALTH,
		POWER,
		RESET
	}

	[SerializeField]
	private Upgrade _upgrade;

	private int _level = 1;

	[SerializeField]
	private Text _levelText;



	protected override void OnExecute(PointerEventData e) {
		switch (_upgrade) {
			case Upgrade.POWER:
			//所持金がレベル足りなかった場合は終了
			if(_manager.Player.MoneyAmount <= (_level*100f)){Debug.Log("Money不足");break;}
				//ステータスの向上
				_playerStats.SkillPowerMultiplyerProperties+= 0.2f;
				//所持金の減少
				_manager.Player.MoneyAmount=_manager.Player.MoneyAmount-(_level*100f);
				//現在のレベルの更新
				_level= (int)((_playerStats.SkillPowerMultiplyerProperties-1)/0.2f+1);
				//テキストの更新
				_levelText.text = _level.ToString();
				
				break;

			case Upgrade.HEALTH:
			//所持金がレベル足りなかった場合は終了
			if(_manager.Player.MoneyAmount <= _level*100f){Debug.Log("Money不足");break;}
				//ステータスの向上
				_playerStats.HealthMultiplyerProperties+= 0.2f;
				//所持金の減少
				_manager.Player.MoneyAmount = _manager.Player.MoneyAmount-(_level*100f);
				//現在のレベルの更新
				_level= (int)((_playerStats.HealthMultiplyerProperties-1)/0.2f+1);
				//テキストの更新
				_levelText.text = _level.ToString();
				
				break;
		
			case Upgrade.RESET:
				//成長ステータスの初期化
				_playerStats.HealthMultiplyerProperties = 1;
				_playerStats.SkillPowerMultiplyerProperties = 1;
				break;
		}
	}
}
