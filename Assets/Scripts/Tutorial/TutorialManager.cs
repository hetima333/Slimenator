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
	private GameObject _lockWall;


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
	private Text[] _countTexts; 

	[SerializeField]
	private Image[] _controllImages; 


	[SerializeField]
	private Sprite[] _controllSprite;
	

	// Use this for initialization
	void Start () {
		//ステップの初期化
		StepUp();
		gameObject.ObserveEveryValueChanged(x => SuckCount).Where(x => (Step==2&&x>=3)).Subscribe(_ => StepUp());

		gameObject.ObserveEveryValueChanged(x => CreateCount).Where(x => (Step==3&&x>=3)).Subscribe(_ => StepUp());

		gameObject.ObserveEveryValueChanged(x => SkillUseCount).Where(x => (Step==4&&x>=3)).Subscribe(_ => StepUp());

		gameObject.UpdateAsObservable().Where(_=>Step == 2).Subscribe(_=>_countTexts[0].text = SuckCount.ToString());

		gameObject.UpdateAsObservable().Where(_=>Step == 3).Subscribe(_=>_countTexts[0].text = CreateCount.ToString());

		gameObject.UpdateAsObservable().Where(_=>Step == 4).Subscribe(_=>_countTexts[0].text = SkillUseCount.ToString());

	
	}
	


	public void StepUp()
	{
		Step++;


		switch(Step)
		{
			case 1:
			if(Language.Instance.language == "Japanese")
			{
				_announceText.text = "マーカーまで移動しよう";
				_controllTexts[0].text ="移動";
				_controllTexts[1].text ="向きの回転";
				
			}
			else
			{
				_announceText.text = "Move to Marker";
				_controllTexts[0].text ="Move";

				_controllTexts[1].text ="Turning";
			}

				_controllImages[0].sprite = _controllSprite[0];
				_controllImages[1].sprite = _controllSprite[2];

			break;

			case 2:
			_countTexts[0].gameObject.SetActive(true);
			_countTexts[1].gameObject.SetActive(true);
			Vector3 Pos = new Vector3(0,1,5);
			GameObject fireSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			fireSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.FIRE);

			Pos = new Vector3(20,1,5);
			GameObject iceSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			iceSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.ICE);

			Pos = new Vector3(-20,1,5);
			GameObject lightningSpowner = ObjectManager.Instance.InstantiateWithObjectPooling(_spawner,Pos);
			lightningSpowner.GetComponent<TutrialSlimeSpowner>().SetType(TutrialSlimeSpowner.SLIME.LIGHTNING);

			if(Language.Instance.language == "Japanese")
			{
				_controllTexts[0].text ="吸い込み";
				_controllTexts[1].text ="向きの回転";
				_announceText.text = "スライムを吸ってみよう";
			}
			else
			{
				_controllTexts[0].text ="suck";
				_controllTexts[1].text ="Turning";
				_announceText.text = "Suck a slime";
			}

			_controllImages[0].sprite = _controllSprite[3];
			_controllImages[1].sprite = _controllSprite[2];
			
			break;

			case 3:
			if(Language.Instance.language == "Japanese")
			{
				_announceText.text = "スキルを作ろう";
				_controllTexts[0].text ="スライムセット";
				_controllTexts[1].text ="スキル生成";
			}
			else
			{
				_announceText.text = "Make skill from slime";
				_controllTexts[0].text ="Set of slime";
				_controllTexts[1].text ="Skill generation";
				
			}

			_controllImages[0].sprite = _controllSprite[1];
			_controllImages[1].sprite = _controllSprite[4];

			break;

			case 4:
			if(Language.Instance.language == "Japanese")
			{
				_announceText.text = "スキルを使ってみよう";
				_controllTexts[0].text ="スキル選択";
				_controllTexts[1].text ="スキル使用";
			}
			else
			{
				_announceText.text = "Let's use skills";
				_controllTexts[0].text ="Select skill";
				_controllTexts[1].text ="Use skills";
			}


			_controllImages[0].sprite = _controllSprite[5];
			_controllImages[1].sprite = _controllSprite[6];

			break;

			case 5:
			_controllImages[0].gameObject.transform.parent.gameObject.SetActive(false);
			_countTexts[0].gameObject.SetActive(false);
			_countTexts[1].gameObject.SetActive(false);

			if(Language.Instance.language == "Japanese")
			{
				_announceText.text = "チュートリアル終了";	
			}
			else
			{
				_announceText.text = "Tutorial End";	
			}
			_lockWall.SetActive(false);
			_scenePortal.SetActive(true);
			break;
		}
	}
}
