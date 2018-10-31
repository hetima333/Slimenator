using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Status Effect/Status")]
public class StatusEffect : ScriptableObject
{
    [SerializeField]
    private List<Effect>
        _EffectList = new List<Effect>();

    [SerializeField]
    private GameEvent
        _Event;

    [SerializeField]
    private ElementType
        _Element;

    [SerializeField]
    private float
       _Timer;

    [SerializeField]
    private GameObject
       _Particle;

    public List<Effect> GetEffectList()
    {
        return _EffectList;
    }

    public ElementType GetElement()
    {
        return _Element;
    }

    public float GetTimer()
    {
        return _Timer;
    }

    public GameObject GetParticle()
    {
        return _Particle;
    }

    public GameEvent GetEvent()
    {
        return _Event;
    }
}
