/// ボスのテストタイプ
/// Boss Test type
/// Athor： Yuhei Mastumura
/// Last edit date：2018/11/07
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestBoss : BossBase, IDamageable {

    //TODO Boss Performance
    const float MAX_HP = 3000.0f;
    const float MONEY = 2000.0f;

    //分裂時生成するボスオブジェクト
    [SerializeField]
    private GameObject Boss1;

    [SerializeField]
    private GameObject Boss2;

    // Use this for initialization
    void Start () {
        SetStatus ();
        PhaseUp ();
        _target = GameObject.Find ("Player");

    }

    // Update is called once per frame
    void Update () {

        _actInterval -= Time.deltaTime; {
            if (_actInterval <= 0) {
                _actInterval = ACT_INTERVAL;
                UseSkill ();
            }
        }

        if (Input.GetKeyDown (KeyCode.C)) {
            PhaseUp ();
        }

    }

    private void SetStatus () {
        _hp = _maxHp;
    }

    //ダメージを受ける
    public void TakeDamage (float damage) {
        _hp -= damage;

        if (_hp <= 0) {
            PhaseUp ();
        }
    }

    private void UseSkill () {

        if (_isAction) { Debug.Log ("今忙しい"); return; }

        foreach (var skill in _skillList) {
            if (skill._canActive == true) {
                _canUseSkillList.Add (skill);
            }
        }

        if (_canUseSkillList.Count == 0) { Debug.Log ("今使えるの無い"); return; }

        Vector3 lookPos = _target.transform.position;

        lookPos.y = gameObject.transform.position.y;

        transform.LookAt (lookPos);

        //ランダムなスキルを呼び出す。
        if (_canUseSkillList.Count != 0) {
            var skill = _canUseSkillList[Random.Range (0, _canUseSkillList.Count)];
            //一回前に使ったスキルではない。かつ発動できる場合実行
            if (skill != _previousSkill) {
                skill.Action ();
                _previousSkill = skill;
                _canUseSkillList.Remove (skill);

            } else {
                Debug.Log ("被っとるんじゃ");
            }
        }

        _canUseSkillList.Clear ();
    }

    private void PhaseUp () {
        _phase++;
        Debug.Log ("Phase" + _phase);

        switch (_phase) {
            case 1:
                //新しいコンポーネントの追加
                _skillList.Add (gameObject.AddComponent<JumpPress> ());
                _skillList.Add (gameObject.AddComponent<FrontSlimeShot> ());
                break;
            case 2:
                _skillList.Add (gameObject.AddComponent<AroundSlimeShot> ());
                _skillList.Add (gameObject.AddComponent<Tackle> ());
                break;
            case 3:
                //分裂
                StartCoroutine (Split ());
                break;
            default:

                break;
        }

    }

    private IEnumerator Split () {

        Debug.Log ("分裂");

        yield return new WaitForSeconds (3);
        Vector3 Pos = gameObject.transform.position;

        Vector3 OffSet = new Vector3 (10, 0, 0);

        GameObject BossA = ObjectManager.Instance.InstantiateWithObjectPooling (Boss1);
        BossA.transform.position = Pos + OffSet;

        GameObject BossB = ObjectManager.Instance.InstantiateWithObjectPooling (Boss2);
        BossA.transform.position = Pos - OffSet;

        BossA.GetComponent<BossTwins> ()._target = _target;
        BossA.GetComponent<BossTwins> ().SetAvatar (BossB);

        BossB.GetComponent<BossTwins> ()._target = _target;
        BossB.GetComponent<BossTwins> ().SetAvatar (BossA);

        Destroy (gameObject);
        //ObjectManager.Instance.ReleaseObject (gameObject);
    }

}