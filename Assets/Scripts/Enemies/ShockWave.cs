using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {

    //パーティクルの生存時間
    //Particle's living time
    float _aliveTime = 3.0f;

    void Update()
    {
        _aliveTime -= Time.deltaTime;

        if (_aliveTime <= 0)
        {
            Destroy(gameObject);
        }
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
            obj.GetComponent<IDamageable>().TakeDamage(1);
            
        }

        //パーティクルのコリジョンの解除（多段ヒット防止）
        //Release of particle community (multistage hit prevention)
        parCol.enabled = false;
    }
}
