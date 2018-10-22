using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public abstract void Init(Vector3 _dir, float _speed);
}
