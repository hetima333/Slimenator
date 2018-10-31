using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : Projectile
{
    public override void Init(Vector3 dir, float speed, ProjectileProperties projectile_properties, GameObjectList Targetable, float timer = 5, float damage = 1, float multiplyer = 1)
    {
        _Dir = dir;
        _speed = speed;
        _timer = timer;
        _damage = damage;
        _multiplyer = multiplyer;
        _ProjectileProperties = projectile_properties;

        if (projectile_properties.GetMovingParticle() != null)
        {
            _MovingParticleCopy = Instantiate(projectile_properties.GetMovingParticle(), gameObject.transform);
        }

        if (projectile_properties.GetImpactParticle() != null)
        {
            ParticleInterface temp = projectile_properties.GetImpactParticle().GetComponent<ParticleInterface>();
            temp.Init();
            _impact_particle_timer = temp.GetLongestParticleEffect();
        }

        if (_Targetable.Count > 0)
            _Targetable.Clear();
        _Targetable.AddRange(Targetable.GetList());
    }

    // Update is called once per frame
    void Update ()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;

        gameObject.transform.position += _Dir.normalized * _speed * Time.deltaTime;

        if (_timer <= 0)
            Dead();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag.Equals("Hittable"))
        {
            Dead();
        }
    }
}
