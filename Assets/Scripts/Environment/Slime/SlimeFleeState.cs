using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFleeState : SlimeBaseState
{
    GameObject
        _tempPlayer;

    Vector3
        _destination;

    public SlimeFleeState(SlimeBase owner) : base(owner)
    {
        _tempPlayer = GameObject.Find("Player");
    }

    public override void OnStateEnter()
    {
        _destination = GetNewDestination();
    }

    public override void OnStateExit()
    {
    }

    public override void Tick()
    {
        if (!Escaped())
            //if (ReachedDestination())
                _destination = GetNewDestination();

        else
            _owner.SetState(new SlimeIdleState(_owner));


        _owner.MoveToward(_owner.GetPosition() + _destination);
    }

    private Vector3 GetNewDestination()
    {
        Vector3 dir = _owner.GetPosition() - _tempPlayer.transform.position;
        return dir * 15f;
    }

    //private bool ReachedDestination()
    //{
    //    return Vector3.Distance(_owner.GetPosition(), _destination) < 0.5f;
    //}

    private bool Escaped()
    {
        return Vector3.Distance(_owner.GetPosition(), _tempPlayer.transform.position) > 7.5f;
    }
}
