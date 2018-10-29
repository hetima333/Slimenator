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
    void OnCollisionEnter (Collision col) {
        //Make sure the target has components
        var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

        //If have a component
        if (hasIDamageableObject != null) {
            //ダメージ判定  
            hasIDamageableObject.TakeDamage (_damage);
        }
    }
}