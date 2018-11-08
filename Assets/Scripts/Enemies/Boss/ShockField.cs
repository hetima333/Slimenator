/// 衝撃フィールドの判定スクリプト
/// Shock Field
/// Athor：Yuhei Mastumura
/// Last edit date：2018/10/24
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockField : MonoBehaviour {

    [SerializeField]
    private float _maxScale;

    [SerializeField]
    private float _increasingRate;

    //ヒット時に与えるダメージ
    [SerializeField]
    private float _damage;

    void OnEnable () {
        gameObject.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
    }

    void Update () {

        if (gameObject.transform.localScale.x < _maxScale) {
            //Change Scale
            float x = gameObject.transform.localScale.x + _increasingRate;
            float y = gameObject.transform.localScale.y + _increasingRate;
            float z = gameObject.transform.localScale.z + _increasingRate;
            gameObject.transform.localScale = new Vector3 (x, y, z);

            //Scale Over
            if (gameObject.transform.localScale.x >= _maxScale) {
                ObjectManager.Instance.ReleaseObject (gameObject);
            }
        }
    }

    public void SetDamage (float damage) {
        _damage = damage;
    }

    public void SetScale (float scale) {
        //Change Object Scale
        _maxScale = scale;

    }

    void OnTriggerEnter (Collider col) {
        Debug.Log (col.gameObject.name);
        //Make sure the target has components
        var hasIDamageableObject = col.gameObject.GetComponent<IDamageable> ();
        //If have a IDamageable component
        if (hasIDamageableObject != null) {
            //ダメージ判定
            //TODO take damage   
            hasIDamageableObject.TakeDamage (_damage);
        }
    }
}