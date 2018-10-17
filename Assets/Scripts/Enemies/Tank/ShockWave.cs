/// 衝撃波の処理
/// Processing enemy's shockwave
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {

    //パーティクルの生存時間
    //Particle's living time
    private   float _aliveTime = 3.0f;

    private float _damage;

    void Update()
    {
        _aliveTime -= Time.deltaTime;

        if (_aliveTime <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetScale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    void OnParticleCollision(GameObject obj)
    {

        //Make sure the target has components
        var hasIDamageable = obj.gameObject.GetComponent<IDamageable>();

        var parCol = gameObject.GetComponent<ParticleSystem>().collision;

        //If have a component
        if (hasIDamageable != null)
        {
            //ダメージ判定
            //TODO take damage   
            obj.GetComponent<IDamageable>().TakeDamage(_damage);
            
        }

        //パーティクルのコリジョンの解除（多段ヒット防止）
        //Release of particle community (multistage hit prevention)
        parCol.enabled = false;
    }
}
