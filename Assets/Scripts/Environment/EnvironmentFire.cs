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

    public void OnParticleCollision(GameObject other)
    {
        int collCount = _pSystem.GetSafeCollisionEventSize();

        if (collCount > _collisionEvents.Count)
        {
            _collisionEvents = new List<ParticleCollisionEvent>();
        }
        int eventCount = _pSystem.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < eventCount; i++)
        {
            Debug.Log("Burn");
            IDamageable temp = other.GetComponent<IDamageable>();
            temp.TakeDamage(2);
            //TODO: Do collision stuff here. 
            // You can access the _collisionEvents[i] to obtaion point of intersection, normals that kind of thing
            // You can simply use "other" GameObject to access it's rigidbody to apply force, or check if it implements a class that takes damage or whatever
        }
    }

    public override void InitObjectWithLife(float lifetime, Vector3 pos, Vector3 size, bool isStatic = true)
    {
        _pSystem.Stop();
        base.InitObjectWithLife(lifetime, pos, size);
        _pSystemMain.duration = lifetime * 0.5f;
        _pSystem.Play();

        Debug.Log(_pSystem.main.duration);
    }
}
