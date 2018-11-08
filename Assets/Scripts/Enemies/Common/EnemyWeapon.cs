/// 敵の武器（爪なども含む）の判定スクリプト
/// Enemy Weapon
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/31
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (BoxCollider))]

public class EnemyWeapon : MonoBehaviour {
    //与えるダメージ
    [SerializeField]
    private float _damage;
    //地面との判定を取るか否か
    [SerializeField]
    private bool _groundHit = false;
    //衝撃波
    GameObject _shockWave;
    //一度当たったオブジェクトを記憶するHashSet（多段ヒット防止）
    private HashSet<GameObject> _hitObjects;

    void Start () {
        //衝撃波オブジェクトのロード
        _shockWave = Resources.Load ("EnemyItem/ShockWave", typeof (GameObject)) as GameObject;
        _hitObjects = new HashSet<GameObject> ();
    }

    public void SetDamage (float damage) {
        _damage = damage;
    }

    //collision swich
    public void ActiveCollision (bool isActive) {
        var collider = GetComponent<BoxCollider> ();
        if (isActive == true) {
            collider.enabled = true;
        } else if (isActive == false) {
            collider.enabled = false;
            HashReset ();
        }
    }

    //自分の本体に何かが接触した場合
    void OnTriggerEnter (Collider col) {

        //Debug.Log (col.gameObject.name);
        //Make sure the target has components
        var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

        //If have a component
        if (hasIDamageableObject != null) {
            //ダメージ判定  
            //当たったことの無いオブジェクトだった場合
            if (!_hitObjects.Contains (col.gameObject)) {
                //ダメージを与える
                hasIDamageableObject.TakeDamage (_damage);
                //一度当たったオブジェクトリストに追加
                _hitObjects.Add (col.gameObject);
            }
        }

        //地面接触時に衝撃波を発生させる
        if (LayerMask.LayerToName (col.gameObject.layer) == "Ground" && _groundHit) {

            if (_shockWave) {
                GameObject shockWave = Instantiate (_shockWave);
                shockWave.GetComponent<ShockWave> ().SetScale (20);
                shockWave.GetComponent<ShockWave> ().SetDamage (10);
                //接触地点を取得
                Vector3 ShockPos = col.ClosestPointOnBounds (this.transform.position);
                ShockPos.y = 0.1f;
                shockWave.transform.position = ShockPos;
            }
        }
    }

    void OnTriggerExit (Collider col) {

    }

    //Reset HashSet
    public void HashReset () {
        _hitObjects.Clear ();
    }

}