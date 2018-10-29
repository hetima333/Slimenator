using System;
using UnityEngine;

public class EnvironmentDestructible : EnvironmentBase, IDamageable {

    [SerializeField]
    GameObject
        _shatterObject;

    float
        _health,
        _maxHealth;

    public float MaxHitPoint
    {
        get
        {
            return _maxHealth;
        }
    }

    public float HitPoint
    {
        get
        {
            return _health;
        }
    }


    // Use this for initialization
    protected override void Start() {
		
	}

    // Update is called once per frame
    protected override void Update () {
	}

    void Die()
    {
        gameObject.SetActive(false);
        EnvironmentBase temp = ObjectManager.Instance.InstantiateWithObjectPooling(_shatterObject, transform.position, transform.rotation).GetComponent<EnvironmentBase>();
        temp.InitObjectWithLife(5);
    }

    public void TakeDamage(float Damage)
    {
        Debug.Log("OUHCHC");
        _health -= Damage;

        if (_health < 0)
        {
            Die();
        }
    }
}
