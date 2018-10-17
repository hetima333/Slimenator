using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : ScriptableObject
{
    ///TODO ADD MESH AND MATERIAL SERIALIZABLE FIELD SO CAN CHANGE

    public virtual void OnMoving(GameObject go)
    { }

    public virtual void OnImpact(GameObject go)
    { }

    public virtual void OnDead(GameObject go)
    { }
}
