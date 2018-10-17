using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Skill")]
public class SkillProjectile : ScriptableObject
{
    [SerializeField]
    private ElementType[]
         _Combination = new ElementType[3];

    [SerializeField]
    private ElementType
        _Base;

    [SerializeField]
    private ProjectileType
        _Type;

    [SerializeField]
    private List<ProjectileProperties>
        _Properties = new List<ProjectileProperties>();

    [SerializeField]
    private float
        _LifeTime;

    // Update is called once per frame
    void Update ()
    {
		
	}
}
