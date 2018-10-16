using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/StraightProjectile")]
public class StraightProjectile : ProjectileType
{

    public override void MovePosition(GameObject go, Vector3 dir, float speed)
    {
        go.transform.position += dir * speed * Time.deltaTime;
    }
	
}
