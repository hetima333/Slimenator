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
        _owner.Animator.SetBool("Moving", true);
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
        return dir * _owner.Stats.MovementRange;
    }

    //private bool ReachedDestination()
    //{
    //    return Vector3.Distance(_owner.GetPosition(), _destination) < 0.5f;
    //}

    // ENG: Check distance between the slime and player.
    // JAP: スライムとプレイヤーの距離を確認してください。
    private bool Escaped()
    {
        if (_tempPlayer != null)
        {
            return Vector3.Distance(_owner.GetPosition(), _tempPlayer.transform.position) > 7.5f;
        }

        return false;
    }
}
