using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager> {


	public int Step = 0;

	[SerializeField]
	private GameObject _spawner;

	[SerializeField]
	private GameObject _testEnemy;

	[SerializeField]
	private GameObject _scenePortal;


	[SerializeField]
	private Text _announceText;

	[SerializeField]
	public int SuckCount;

	[SerializeField]
	public int CreateCount;

	[SerializeField]
	public int SkillUseCount;


	[SerializeField]
	private Text[] _controllTexts; 

	[SerializeField]
	private Image[] _controllImages; 
	

	// Use this for initialization
	void Start () {
		//ステップの初期化
		StepUp();
		gameObject.ObserveEveryValueChanged(x => SuckCount).Where(x => (Step==2&&x>3)).Subscribe(_ => StepUp());

		gameObject.ObserveEveryValueChanged(x => CreateCount).Where(x => (Step==3&&x>3)).Subscribe(_ => StepUp());

		gameObject.ObserveEveryValueChanged(x => SkillUseCount).Where(x => (Step==4&&x>3)).Subscribe(_ => StepUp());
	}
	


	public void StepUp()
	{
		Step++;


		switch(Step)
		{
			case 1:
				_announceText.text = "マーカーまで移動しよう";
				_controllTexts[0].text ="移動";
				_controllTexts[1].text ="向きの回転";
			break;

			case 2:
			Vector3 Pos = new Vector3(0,1,0);
			GameObject fireSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			fireSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.FIRE);

			Pos = new Vector3(20,1,0);
			GameObject iceSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			iceSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.ICE);

			Pos = new Vector3(-20,1,0);
			GameObject lightningSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			lightningSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.LIGHTNING);

			_controllTexts[0].text ="吸い込み";
			_controllTexts[1].text ="向きの回転";
				
				_announceText.text = "スライムを吸ってみよう";
			break;

			case 3:
				_announceText.text = "スライムを使ってスキルを作ろう";
				_controllTexts[0].text ="スライムのセット";
				_controllTexts[1].text ="スキル生成";
			break;

			case 4:
				_announceText.text = "スキルを使ってみよう";
				_controllTexts[0].text ="スキル選択";
				_controllTexts[1].text ="スキル使用";
			break;

			case 5:
				_announceText.text = "Tutorial終了";
				_scenePortal.SetActive(true);
			break;
		}
	}
}
