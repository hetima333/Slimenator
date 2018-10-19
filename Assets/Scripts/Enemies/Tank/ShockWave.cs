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

    //ヒット時に与えるダメージ
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
        //Change Particle　Scale
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        //Change Particle collision radius
        var parCol = gameObject.GetComponent<ParticleSystem>().collision;
        parCol.radiusScale = scale;
    }

    void OnParticleCollision(GameObject obj)
    {

        Debug.Log(obj.name);

        //Make sure the target has components
        var hasIDamageable = obj.gameObject.GetComponent<IDamageable>();

        //If have a IDamageable component
        if (hasIDamageable != null)
        {
            //ダメージ判定
            //TODO take damage   
            obj.GetComponent<IDamageable>().TakeDamage(_damage);
            
        }

        //パーティクルのコリジョンの解除（多段ヒット防止）
        //Release of particle community (multistage hit prevention)
        var parCol = gameObject.GetComponent<ParticleSystem>().collision;
        parCol.enabled = false;
    }

}
