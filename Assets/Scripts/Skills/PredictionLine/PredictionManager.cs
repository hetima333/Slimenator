using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PredictionManager : MonoBehaviour
{
    float _rate;

    // それぞれの予測線オブジェクトを管理
    [SerializeField]
    private GameObject _linePrediction;
    [SerializeField]
    private GameObject _sectorPrediction;
    [SerializeField]
    private GameObject _circlePrediction;

    static private float SPRITE_RATE = 0.6f;
    
    // Use this for initialization
    void Start()
    {
        _linePrediction.SetActive(false);
        _circlePrediction.SetActive(false);
        _sectorPrediction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Modeを外部から切り替える
    public void SwitchMode(Skill skill)
    {
        _linePrediction.SetActive(false);
        _circlePrediction.SetActive(false);
        _sectorPrediction.SetActive(false);

        // Skillが設定されていないとき
        if (skill == null)
            return;

        // スキルの型を取得
        print(skill.GetType());
        // 型に応じて範囲を設定
        var projectile = skill as SkillProjectile;
        var target = skill as SkillTargeted;
        var spawning = skill as SkillSpawning;
        var skillEvent = skill as SkillEvent;




        // ユニークでないときのレベルに応じた範囲補正
        if(!skill.IsUnique())
        {
            _rate = skill.GetSkillTier().GetMultiplyer();
        }

        var use_rate = SPRITE_RATE ;

        float area;
        // 現在の技に応じて有効な予測線を決定
        switch (skill.name)
        {
            // 線形
            case "Fire Ball":
            case "Vacuum Bomb":
            case "Slime Bazooka":

                _linePrediction.SetActive(true);
                break;

            // 円形
            case "Lightning Strike":
                var lightning = target.GetCastType() as SkillTargetAOESelf;
                area = lightning.GetRange() * _rate;
                _circlePrediction.gameObject.transform.localScale = new Vector3(area * use_rate, area * use_rate, 1);
                _circlePrediction.SetActive(true);
                break;

            case "Slime Nuke":
                var nuke = spawning.GetCastType() as SkillTargetAOESelf;
                area = nuke.GetRange();
                _sectorPrediction.gameObject.transform.localScale = new Vector3(area * use_rate, area * use_rate, 1);
                _circlePrediction.SetActive(true);
                break;

            case "Slime Rain":
                var rain = spawning.GetCastType() as SkillTargetSelf;
                area = rain.GetRange();
                _sectorPrediction.gameObject.transform.localScale = new Vector3(area * use_rate, area * use_rate, 1);
                _circlePrediction.SetActive(true);
                break;

            case "Enlargement":
                var big = spawning.GetCastType() as SkillTargetSelf;
                area = big.GetRange();
                _sectorPrediction.gameObject.transform.localScale = new Vector3(area * use_rate, area * use_rate, 1);
                _circlePrediction.SetActive(true);
                break;

            // 扇形
            case "Frost Blast":
                var frost = target.GetCastType() as SkillTargetCone;
                area = frost.GetRange() * _rate;
                _sectorPrediction.gameObject.transform.localScale = new Vector3(area * use_rate, area * use_rate, 1);
                _sectorPrediction.SetActive(true);
                break;

            case "Giga Vacuum":
                var self = skillEvent.GetCastType() as SkillTargetSelf;
                area = self.GetRange();
                _sectorPrediction.gameObject.transform.localScale = new Vector3(8, 8, 1);
                _sectorPrediction.SetActive(true);
                break;
            
            default:
                break;
        }

    }
}
