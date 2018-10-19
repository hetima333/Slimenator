using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockField : MonoBehaviour {
    //パーティクルの生存時間
    //Particle's living time
    [SerializeField]
    private float _aliveTime = 3.0f;

    [SerializeField]
    private float _maxScale;

    [SerializeField]
    private float _increasingRate;

    //ヒット時に与えるダメージ
    private float _damage;

    void Update()
    {
        _aliveTime -= Time.deltaTime;

        if (_aliveTime <= 0)
        {
            Destroy(gameObject);
        }


        if (gameObject.transform.localScale.x < _maxScale)
        {
            float x = gameObject.transform.localScale.x + _increasingRate;
            float y = gameObject.transform.localScale.y + _increasingRate;
            float z = gameObject.transform.localScale.z + _increasingRate;
            gameObject.transform.localScale = new Vector3(x,y,z);
        }
    }


    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetScale(float scale)
    {
        //Change Particle Scale
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        //Change Particle collision radius
        var parCol = gameObject.GetComponent<ParticleSystem>().collision;
        parCol.radiusScale = scale;
    }

    void OnTrrigerEnter(Collider col)
    {

        Debug.Log(col.gameObject.name);

        //Make sure the target has components
        var hasIDamageable = col.gameObject.GetComponent<IDamageable>();

        //If have a IDamageable component
        if (hasIDamageable != null)
        {
            //ダメージ判定
            //TODO take damage   
            col.gameObject.GetComponent<IDamageable>().TakeDamage(_damage);
        }

    }
}
