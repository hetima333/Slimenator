using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector3
     _Dir;

    protected float
        _speed,
        _timer,
        _damage;

    protected List<StatusEffect>
        _StatusEffects = new List<StatusEffect>();

    protected List<GameObject>
       _Targetable = new List<GameObject>();

    protected GameObject
        _MovingParticleCopy;

    protected ProjectileProperties
        _ProjectileProperties;

    protected float
        _impact_particle_timer, 
        _multiplyer;

    public abstract void Init(Vector3 dir, float speed, ProjectileProperties projectile_properties, GameObjectList Targetable, float timer = 5, float damage = 1, float multiplyer = 1);

    public void SetStatusEffect(List<StatusEffect> statusEffects)
    {
        _StatusEffects.AddRange(statusEffects);
    }

    protected virtual void Dead()
    {
        gameObject.SetActive(false);

        if (_MovingParticleCopy != null)
        {
            Destroy(_MovingParticleCopy);
            _MovingParticleCopy = null;
        }

        if (_ProjectileProperties.GetImpactParticle() != null)
        {
            GameObject temp = Instantiate(_ProjectileProperties.GetImpactParticle(), gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.localScale = gameObject.transform.localScale;
            Destroy(temp, _impact_particle_timer);
        }

        foreach (GameObject obj in _Targetable)
        {
            if (ObjectManager.Instance.GetActiveObjects(obj) != null)
            {
                foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                {
                    if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _ProjectileProperties.GetImpactRadius() * _multiplyer)
                    {
                        IDamageable dmg = entity.GetComponent<IDamageable>();

                        if (dmg != null)
                        {
                            dmg.TakeDamage(_damage);

                            foreach (StatusEffect se in _StatusEffects)
                            {
                                se.GetEvent().InvokeSpecificListner(obj.GetInstanceID());
                            }
                        }
                    }
                }
            }
        }
    }
}