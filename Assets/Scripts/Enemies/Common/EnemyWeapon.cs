/// 敵の武器（爪なども含む）の判定スクリプト
/// Enemy Weapon
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/24
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (BoxCollider))]

public class EnemyWeapon : MonoBehaviour {
    [SerializeField]
    float _damage;

    GameObject _shockWave;

    void Start () {
        //衝撃波オブジェクトのロード
        _shockWave = Resources.Load ("EnemyItem/ShockWave", typeof (GameObject)) as GameObject;
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
            hasIDamageableObject.TakeDamage (_damage);
        }

        //地面接触時に衝撃波を発生させる
        if (LayerMask.LayerToName (col.gameObject.layer) == "Ground") {

            if (_shockWave) {
                GameObject shockWave = Instantiate (_shockWave);
                shockWave.GetComponent<ShockWave> ().SetDamage (10);
                Vector3 ShockPos = col.ClosestPointOnBounds (this.transform.position);
                ShockPos.y = 0.1f;
                shockWave.transform.position = ShockPos;
            }

        }

    }

    void OnTriggerExit (Collider col) {

    }

}