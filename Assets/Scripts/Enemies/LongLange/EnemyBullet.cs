/// 敵の弾薬処理
/// Processing enemy's bullets
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    private const float ALIVE_TIME = 3.0f;

    private float _aliveTime = ALIVE_TIME;

    [SerializeField]
    private float _damage;

    void OnEnable () {
        _aliveTime = ALIVE_TIME;
    }

    void Update () {
        _aliveTime -= Time.deltaTime;

        if (_aliveTime <= 0) {
            Destroy (gameObject);
        }
    }

    public void SetDamage (float damage) {
        _damage = damage;
    }

    public void SetScale (float scale) {
        gameObject.transform.localScale = new Vector3 (scale, scale, scale);
    }

    //自分の本体に何かが接触した場合
    void OnCollisionEnter (Collision col) {

        //Make sure the target has components
        var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();

        //If have a component
        if (hasIDamageableObject != null) {
            //ダメージ判定
            //TODO take damage   
            hasIDamageableObject.TakeDamage (_damage);
        }

        //Release bullet
        ObjectManager.Instance.ReleaseObject (gameObject);
    }
}