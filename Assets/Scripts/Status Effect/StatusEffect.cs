using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Status")]
public class StatusEffect : ScriptableObject
{
    [SerializeField]
    private float
        _Timer;

    [SerializeField]
    private List<Effect>
        _EffectList = new List<Effect>();

    [SerializeField]
    private GameObject
        _Particle;

    public void UpdateStatusEffect()
    {
        if (_Timer > 0)
            _Timer -= Time.deltaTime;
    }

    public bool IsStatusEffectDone()
    {
        return _Timer <= 0;
    }
	
}
