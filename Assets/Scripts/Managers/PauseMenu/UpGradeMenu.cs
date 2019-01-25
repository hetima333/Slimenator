using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class UpGradeMenu : MonoBehaviour {

//所持金テキスト
[SerializeField]
private Text _moneyText;

private float _oldMoney = 0;
//体力LVテキスト
[SerializeField]
private Text _helthLevelText;
//スキルLVテキスト
[SerializeField]
private Text _skillLevelText;

[SerializeField]
private Stats _playerStats;
	

	void  OnEnable()
	{
		//持ち金の設定
		_oldMoney = GetComponent<MoneyCore>().MoneyAmount;
		_moneyText.text = _oldMoney.ToString();
	}


	void Start () {

		// 所持金のチェックストリーム
		var money = GetComponent<MoneyCore>();
		gameObject.ObserveEveryValueChanged(x => money.MoneyAmount)
		.Subscribe(x => {
			var addMoney = money.MoneyAmount - _oldMoney;
			if(Pausable.Instance.Pausing)
			{
				//所持金の表示値の更新
				StartCoroutine(ScoreAnimation(_moneyText,_oldMoney,addMoney,1));
				_oldMoney = money.MoneyAmount;
			}	
			});

		//体力レベルテキスト更新(仮)
		_playerStats.ObserveEveryValueChanged(x => _playerStats.HealthMultiplyerProperties)
		.Subscribe(_=>_helthLevelText.text = ((int)((_playerStats.HealthMultiplyerProperties-1)/0.2f+1)).ToString());

		//体力レベルテキスト更新（仮）
		_playerStats.ObserveEveryValueChanged(x => _playerStats.SkillPowerMultiplyerProperties)
		.Subscribe(_=>_skillLevelText.text = ((int)((_playerStats.SkillPowerMultiplyerProperties-1)/0.2f+1)).ToString());
		
	}



	// スコアをアニメーションさせる
    IEnumerator ScoreAnimation(Text target,float oldScore, float addScore, float time)
    {
        //前回のスコア
        float befor = oldScore;
        //今回のスコア
        float after = oldScore + addScore;
        //0fを経過時間にする
        float elapsedTime = 0.0f;
        //timeが０になるまでループさせる
        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            // テキストの更新
            target.text = (befor + (after - befor) * rate).ToString("f0");

            elapsedTime += Time.unscaledDeltaTime;
            // 0.01秒待つ
            yield return new WaitForSecondsRealtime(0.01f);
        }
        // 最終的な着地のスコア
        target.text = after.ToString();
    }
	
}

