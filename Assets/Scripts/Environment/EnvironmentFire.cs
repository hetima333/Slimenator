using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFire : EnvironmentBase {
    [SerializeField]
    private ParticleSystem _pSystem;
    private ParticleSystem.MainModule _pSystemMain;
    private List<ParticleCollisionEvent> _collisionEvents;

    protected override void Awake() {
        _pSystem = GetComponentInChildren<ParticleSystem>();
        _pSystemMain = _pSystem.main;
    }

    // Use this for initialization
    protected override void Start () {

    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    public override void InitObjectWithLife(float lifetime, Vector3 pos, Vector3 size, bool isStatic = true)
    {
        _pSystem.Stop();
        base.InitObjectWithLife(lifetime, pos, size);
        _pSystemMain.duration = lifetime * 0.5f;
        _pSystem.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(2.0f);
        }
    }
}
