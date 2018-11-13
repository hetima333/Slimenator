using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    private SpawningProperties
        _Properties;

    private float
        _LifeTime, 
        _Delay;

    public void Init(SpawningProperties Properties)
    {
        _Properties = Properties;
        ParticleInterface ParticlePI = _Properties.GetParticle().GetComponent<ParticleInterface>();
        ParticlePI.Init();
        _LifeTime = ParticlePI.GetLongestParticleEffect();
        GameObject temp = Instantiate(_Properties.GetParticle(), gameObject.transform);
        Destroy(temp, _LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (_LifeTime <= 0)
            gameObject.SetActive(false);
        else
            _LifeTime -= Time.deltaTime;

        switch (_Properties.GetEffectType())
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
                                DestroyImmediate(temp_component);

                            int type = Random.Range(0, _Properties.GetElement().GetList().Count);

                            System.Type _MyScriptType = System.Type.GetType(((ElementType)_Properties.GetElement().GetList()[type]).GetSlimeScriptName());
                            temp_obj.AddComponent(_MyScriptType);

                            temp_obj.GetComponent<SlimeBase>().Init(temp, ((((ElementType)_Properties.GetElement().GetList()[type]).name.Equals("Lightning")) ? 2 : 1), ((ElementType)_Properties.GetElement().GetList()[type]));
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
