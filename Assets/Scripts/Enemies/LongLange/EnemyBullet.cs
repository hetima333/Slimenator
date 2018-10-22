﻿/// 敵の弾薬処理
/// Processing enemy's bullets
/// Athor：　Yuhei Mastumura
/// Last edit date：2018/10/17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    float _aliveTime = 3.0f;

    float _damage;

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


    //自分の本体に何かが接触した場合
    void OnCollisionEnter(Collision col)
    {

        //Make sure the target has components
        var hasIDamageable = col.gameObject.GetComponent<IDamageable>();

        //If have a component
        if (hasIDamageable!=null)
        {
            //ダメージ判定
            //TODO take damage   
            col.gameObject.GetComponent<IDamageable>().TakeDamage(_damage);
        }

        //Delete bullet
        Destroy(gameObject);
    }
}
