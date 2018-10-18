using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFire : EnvironmentBase {
    private ParticleSystem _pSystem;
    private List<ParticleCollisionEvent> _collisionEvents;

    // Use this for initialization
    protected override void Start () {
        _pSystem = GetComponent<ParticleSystem>();
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
            //TODO: Do your collision stuff here. 
            // You can access the _collisionEvents[i] to obtaion point of intersection, normals that kind of thing
            // You can simply use "other" GameObject to access it's rigidbody to apply force, or check if it implements a class that takes damage or whatever
        }
    }

    //public override void SetUpObjectWLifeTime(float lifetime, Vector3 pos, Vector3 size, bool isStatic = true)
    //{
    //    base.SetUpObjectWLifeTime(lifetime, pos, size, isStatic);
    //}
}
