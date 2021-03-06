﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    private SpawningProperties
        _Properties;

    private float
        _StartingTime, 
        _EndingTime,
        _Delay;

    private bool
        _SpawnedEnding,
        _PlayedCastingAudio,
        _PlayedEndingAudio;

    public void Init(SpawningProperties Properties)
    {
        _SpawnedEnding = false;
        _PlayedCastingAudio = _PlayedEndingAudio = false;


        _Properties = Properties;
        ParticleInterface ParticlePI = null;

        if (_Properties.GetStartingParticle() != null)
        {
            ParticlePI = _Properties.GetStartingParticle().GetComponent<ParticleInterface>();
            ParticlePI.Init();
            _StartingTime = ParticlePI.GetLongestParticleEffect();

            GameObject temp = Instantiate(_Properties.GetStartingParticle(), gameObject.transform);
            Destroy(temp, _StartingTime);

            if(!_PlayedCastingAudio)
            {
                if(_Properties.GetCastingAudio() != null)
                {
                    _PlayedCastingAudio = true;
                    AudioManager.Instance.PlaySE(_Properties.GetCastingAudio().name, _Properties.IsCastingLoop());
                }
            }
        }
        else
            _StartingTime = 0;

        if (_Properties.GetEndingParticle() != null)
        {
            _Properties = Properties;
            ParticlePI = _Properties.GetEndingParticle().GetComponent<ParticleInterface>();
            ParticlePI.Init();
            _EndingTime = ParticlePI.GetLongestParticleEffect();
        }
        else
            _EndingTime = 0;

        _Delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_StartingTime <= 0)
        {
            if(!_SpawnedEnding && _Properties.GetEndingParticle() != null)
            {
                GameObject temp = Instantiate(_Properties.GetEndingParticle(), gameObject.transform);
                Destroy(temp, _EndingTime);
                _SpawnedEnding = true;
            }

            if(_PlayedCastingAudio)
            {
                if (_Properties.IsCastingLoop())
                {
                    AudioManager.Instance.StopSE(_Properties.GetCastingAudio().name);
                    _PlayedCastingAudio = false;
                }
            }

            if (!_PlayedEndingAudio)
            {
                if (_Properties.GetEndingAudio() != null)
                {
                    _PlayedEndingAudio = true;
                    AudioManager.Instance.PlaySE(_Properties.GetEndingAudio().name, _Properties.IsEndingLoop());
                }
            }

            if (_EndingTime <= 0)
            {
                gameObject.SetActive(false);

                if (_PlayedEndingAudio)
                {
                    if (_Properties.IsEndingLoop())
                    {
                        AudioManager.Instance.StopSE(_Properties.GetEndingAudio().name);
                        _PlayedEndingAudio = false;
                    }
                }
            }
            else
                _EndingTime -= Time.deltaTime;
        }
        else
            _StartingTime -= Time.deltaTime;

        switch (_Properties.GetEffectType(_StartingTime <= 0))
        {
            case EnumHolder.AreaEffectType.SPAWNING:
                {
                    if (_Delay <= 0)
                    {
                        int temp_target = Random.Range(0, _Properties.GetTargetable().GetList().Count);

                        GameObject temp_obj = ObjectManager.Instance.InstantiateWithObjectPooling(
                            _Properties.GetTargetable().GetList()[temp_target],
                            new Vector3(
                                gameObject.transform.position.x + Random.Range(-_Properties.GetRadius(), _Properties.GetRadius()),
                                gameObject.transform.position.y + _Properties.GetYOffset(),
                                gameObject.transform.position.z + Random.Range(-_Properties.GetRadius(), _Properties.GetRadius())
                            ));

                        if (temp_obj.tag.Equals("Slime"))
                        {
                            Stats temp = EnumHolder.Instance.GetStats(_Properties.GetTargetable().GetList()[temp_target].name);
                            SlimeBase temp_component = temp_obj.GetComponent<SlimeBase>();

                            if (temp_component != null)
                                Destroy(temp_component);

                            int type = Random.Range(0, _Properties.GetElement().GetList().Count);

                            System.Type _MyScriptType = System.Type.GetType(((ElementType)_Properties.GetElement().GetList()[type]).GetSlimeScriptName());
                            SlimeBase temp_script = temp_obj.AddComponent(_MyScriptType) as SlimeBase;

                            temp_script.Init(temp, ((((ElementType)_Properties.GetElement().GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_Properties.GetElement().GetList()[type]), _Properties.GetTier());
                        }

                        _Delay = _Properties.GetDelay();
                    }
                    else
                        _Delay -= Time.deltaTime;
                }
                break;

            case EnumHolder.AreaEffectType.SUCKING:
                {
                    foreach (GameObject obj in _Properties.GetTargetable().GetList())
                    {
                        if (ObjectManager.Instance.GetActiveObjects(obj) != null)
                        {
                            foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                            {
                                if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _Properties.GetRadius())
                                {
                                    entity.transform.position = Vector3.Slerp(entity.transform.position, new Vector3(gameObject.transform.position.x, entity.transform.position.y, gameObject.transform.position.z), (1.0f - (Vector3.Distance(gameObject.transform.position, entity.transform.position) / _Properties.GetRadius())) * 0.5f);
                                }
                            }
                        }
                    }
                }
                break;

            case EnumHolder.AreaEffectType.PUSHING:
                {
                    foreach (GameObject obj in _Properties.GetTargetable().GetList())
                    {
                        if (ObjectManager.Instance.GetActiveObjects(obj) != null)
                        {
                            foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                            {
                                if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _Properties.GetRadius())
                                {
                                    entity.transform.position += (
                                        new Vector3(entity.transform.position.x, entity.transform.position.y, entity.transform.position.z) -
                                        new Vector3(gameObject.transform.position.x, entity.transform.position.y, gameObject.transform.position.z)).normalized *
                                        (1.0f - (Vector3.Distance(gameObject.transform.position, entity.transform.position) / _Properties.GetRadius()));

                                    if (_Delay <= 0)
                                    {
                                        IDamageable dmg = entity.GetComponent<IDamageable>();
                                        if (dmg != null)
                                        {
                                            if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _Properties.GetRadius())
                                            {
                                                dmg.TakeDamage(_Properties.GetDamage());
                                            }
                                        }
                                        _Delay = _Properties.GetDelay();
                                    }
                                    else
                                        _Delay -= Time.deltaTime;
                                }
                            }
                        }
                    }
                }
                break;

            case EnumHolder.AreaEffectType.DAMAGE:
                {
                    if (_Delay <= 0)
                    {
                        foreach (GameObject obj in _Properties.GetTargetable().GetList())
                        {
                            if (ObjectManager.Instance.GetActiveObjects(obj) != null)
                            {
                                foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                                {
                                    IDamageable dmg = entity.GetComponent<IDamageable>();
                                    if (dmg != null)
                                    {
                                        if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _Properties.GetRadius())
                                        {
                                            dmg.TakeDamage(_Properties.GetDamage());
                                           if (LayerMask.LayerToName(entity.layer) == "Slime")
                                            {
                                                entity.SetActive(false);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        _Delay = _Properties.GetDelay();
                    }
                    else
                        _Delay -= Time.deltaTime;
                }
                break;

            case EnumHolder.AreaEffectType.GROWING:
                {
                    if (_Delay <= 0)
                    {
                        foreach (GameObject obj in _Properties.GetTargetable().GetList())
                        {
                            if (ObjectManager.Instance.GetActiveObjects(obj) != null)
                            {
                                foreach (GameObject entity in ObjectManager.Instance.GetActiveObjects(obj))
                                {
                                    IGrowable grow = entity.GetComponent<IGrowable>();
                                    if (grow != null)
                                    {
                                        if (Vector3.Distance(gameObject.transform.position, entity.transform.position) < _Properties.GetRadius())
                                        {
                                            grow.Grow(_Properties.GetTier());
                                        }
                                    }
                                }   
                            }
                        }

                        _Delay = _Properties.GetDelay();
                    }
                    else
                        _Delay -= Time.deltaTime;
                }
                break;

            default:
                break;
        }
	}
}
