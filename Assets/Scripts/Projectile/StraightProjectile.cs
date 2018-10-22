using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    private Vector3
        _Dir;

    private float
        _speed;

    private float
        _timer;

    public override void Init(Vector3 dir, float speed)
    {
        _Dir = dir;
        _speed = speed;
        _timer = 5;
    }

    // Update is called once per frame
    void Update ()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;

        gameObject.transform.position += _Dir.normalized * _speed * Time.deltaTime;

        if (_timer <= 0)
            gameObject.SetActive(false);

    }
}
