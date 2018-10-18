using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileType : ScriptableObject
{
    public virtual void MovePosition(GameObject go, Vector3 dir, float speed)
    { }
}
