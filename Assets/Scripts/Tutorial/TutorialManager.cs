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
	private Text _announceText;

	[SerializeField]
	public int SuckCount;

	[SerializeField]
	public int CreateCount;

	[SerializeField]
	public int SkillUseCount;
	

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
				_announceText.text = "目的地に移動してみよう";
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
				
				_announceText.text = "スライムを吸ってみよう";
			break;

			case 3:
				_announceText.text = "スキルを作ろう";
			break;

			case 4:
				_announceText.text = "スキルを使ってみよう";
			break;

			case 5:
				_announceText.text = "Tutorial終了";
			break;
		}
	}
}
