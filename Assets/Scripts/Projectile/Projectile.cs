using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    private List<StatusEffect>
        _StatusEffects = new List<StatusEffect>();

    public abstract void Init(Vector3 dir, float speed, float timer = 5, float damage = 1);

    public void SetStatusEffect(List<StatusEffect> statusEffects)
    {
        _StatusEffects.AddRange(statusEffects);
    }

}