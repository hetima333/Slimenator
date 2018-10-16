using UnityEngine;

public class SlimeIdleState : SlimeBaseState
{
    GameObject
        _tempPlayer;

    float
       _stateElapsedTime,
       _stateMaxTime;

    public SlimeIdleState(SlimeBase owner) : base(owner)
    {
        _tempPlayer = GameObject.Find("Player");
    }

    public override void OnStateEnter()
    {
        _stateElapsedTime = 0;
        _stateMaxTime = UnityEngine.Random.Range(3.0f, 5.0f);
    }

    public override void OnStateExit()
    {
    }

    public override void Tick()
    {
        if (CheckIsPlayerInRange())
        {
            _owner.SetState(new SlimeFleeState(_owner));
        }

        else if (_stateElapsedTime > _stateMaxTime)
        {
            _owner.SetState(new SlimeWanderState(_owner));
        }
        else
        {
            _stateElapsedTime += Time.deltaTime;
        }
    }

    private bool CheckIsPlayerInRange()
    {
        if (_tempPlayer != null)
        {
            return Vector3.Distance(_owner.GetPosition(), _tempPlayer.transform.position) < 7.0f;
        }

        return false;
    }
}
