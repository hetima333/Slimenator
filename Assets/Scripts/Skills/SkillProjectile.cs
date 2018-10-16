using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Skill")]
public class SkillProjectile : ScriptableObject
{
    [SerializeField]
    private ElementType[]
         Combination = new ElementType[3];

    [SerializeField]
    private ElementType
        Base;

    [SerializeField]
    private ProjectileType
        Type;

    [SerializeField]
    private List<ProjectileProperties>
        Properties = new List<ProjectileProperties>();

	// Update is called once per frame
	void Update ()
    {
		
	}
}
