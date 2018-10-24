using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class EnemyWeapon : MonoBehaviour {

float _damage;

BoxCollider _col;

	// Use this for initialization
	void Start () 
	{
		
	}


	public void SetDamage(float damage)
	{
_damage = damage;

	}
	

}
