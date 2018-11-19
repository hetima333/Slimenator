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
        _damage, 
        _percentage;

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
        _multiplyer, 
        _shaketimer = 0;

    protected bool
      _PlayedCastingAudio,
      _PlayedEndingAudio;

    public abstract void Init(Vector3 dir, float speed, ProjectileProperties projectile_properties, List<StatusEffect> Status, GameObjectList Targetable, float timer = 5, float damage = 1, float multiplyer = 1, float percentage = 0);

    public void SetStatusEffect(List<StatusEffect> statusEffects)
    {
        _StatusEffects.AddRange(statusEffects);
    }

    protected virtual void Update()
    {
        if(_ProjectileProperties.ShakeOnTraveling())
        {
            if(_shaketimer <= 0)
            {
                for (int i = 0; i < _multiplyer; ++i)
                    _ProjectileProperties.GetEvent().InvokeAllListeners();
                _shaketimer = _ProjectileProperties.GetShakeDelay();
            }
        }

        if (!_PlayedCastingAudio)
        {
            if (_ProjectileProperties.GetCastingAudio() != null)
            {
                _PlayedCastingAudio = true;
                AudioManager.Instance.PlaySE(_ProjectileProperties.GetCastingAudio().name, _ProjectileProperties.IsCastingLoop());
            }
        }
    }

    protected virtual void Dead()
    {
        gameObject.SetActive(false);

        if(_PlayedCastingAudio)
        {
            if(_ProjectileProperties.IsCastingLoop())
            {
                AudioManager.Instance.StopSELoop(_ProjectileProperties.GetCastingAudio().name);
                _PlayedCastingAudio = false;
            }
        }

        if (!_PlayedEndingAudio)
        {
            if (_ProjectileProperties.GetEndingAudio() != null)
            {
                _PlayedEndingAudio = true;
                AudioManager.Instance.PlaySE(_ProjectileProperties.GetEndingAudio().name, _ProjectileProperties.IsEndingLoop());
            }
        }

        if (_ProjectileProperties.ShakeOnImpact())
        {
            for (int i = 0; i < _multiplyer; ++i)
                _ProjectileProperties.GetEvent().InvokeAllListeners();
        }

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

        if (_damage > 0)
        {
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
                                dmg.TakeDamage(_damage * _multiplyer);

                                Debug.Log("[Damaging (" + _damage * _multiplyer + ")] " + obj.name);

                                if (_StatusEffects.Count > 0)
                                {
                                    if (Random.Range(0, 100) < _percentage * _multiplyer)
                                    {
                                        foreach (StatusEffect se in _StatusEffects)
                                        {
                                            Debug.Log("[Applying (" + se.name + ")] " + obj.name);
                                            se.GetEvent().InvokeSpecificListner(obj.GetInstanceID());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //SPAWNING
        if (_ProjectileProperties.GetObjectsToSpawn() != null)
        {
            foreach (GameObject obj in _ProjectileProperties.GetObjectsToSpawn().GetList())
            {
                for (int i = 0; i < _ProjectileProperties.GetIteration(); ++i)
                {
                    GameObject temp_obj = ObjectManager.Instance.InstantiateWithObjectPooling(obj, gameObject.transform.position);

                    if (temp_obj.GetComponent<AreaEffect>() != null)
                    {
                        temp_obj.GetComponent<AreaEffect>().Init(_ProjectileProperties.GetProperties());
                    }
                    else if (temp_obj.tag.Equals("Slime"))
                    {
                        Stats temp = EnumHolder.Instance.GetStats(obj.name);
                        SlimeBase temp_component = temp_obj.GetComponent<SlimeBase>();

                        if (temp_component != null)
                            Destroy(temp_component);

                        int type = Random.Range(0, _ProjectileProperties.GetProperties().GetElement().GetList().Count - 1);

                        System.Type _MyScriptType = System.Type.GetType(((ElementType)_ProjectileProperties.GetProperties().GetElement().GetList()[type]).GetSlimeScriptName());
                        SlimeBase temp_script = temp_obj.AddComponent(_MyScriptType) as SlimeBase;

                        temp_script.Init(temp, ((((ElementType)_ProjectileProperties.GetProperties().GetElement().GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_ProjectileProperties.GetProperties().GetElement().GetList()[type]), _ProjectileProperties.GetTier());
                    }

                    if (_ProjectileProperties._HasRandomMotion)
                    {
                        Rigidbody temp_rb = temp_obj.GetComponent<Rigidbody>();

                        if (temp_rb != null)
                        {
                            float force = 3500;
                            temp_rb.AddForce(new Vector3(Random.Range(-force, force), 0, Random.Range(-force, force)), ForceMode.Acceleration);
                        }
                    }
                }
            }
        }
    }
}