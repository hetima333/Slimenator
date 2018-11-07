/// 衝撃波の処理
/// Processing enemy's shockwave
/// Athor： Yuhei Mastumura
/// Last edit date：2018/10/31
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {

    private const float ALIVE_TIME = 3.0f;

    //パーティクルの生存時間
    //Particle's living time
    private float _aliveTime = 3.0f;

    //ヒット時に与えるダメージ
    private float _damage;

    private HashSet<GameObject> _hitObjects;

    void Start () {
        _hitObjects = new HashSet<GameObject> ();
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
        //Change Particle Scale
        gameObject.transform.localScale = new Vector3 (scale, scale, scale);
        //Change Particle collision radius
        var parCol = gameObject.GetComponent<ParticleSystem> ().collision;
        parCol.radiusScale = scale;
    }

    void OnParticleCollision (GameObject obj) {

        //Debug.Log (obj.name);

        //Make sure the target has components
        var hasIDamageableObject = obj.gameObject.GetComponent<IDamageable> ();

        //If have a IDamageable component
        if (hasIDamageableObject != null) {
            //ダメージ判定
            if (!_hitObjects.Contains (obj)) {
                //ダメージを与える
                hasIDamageableObject.TakeDamage (_damage);
                //一度当たったオブジェクトリストに追加
                _hitObjects.Add (obj);
            }

        }

    }

}