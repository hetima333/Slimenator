using UnityEngine;

public class SlimeIdleState : SlimeBaseState
{
    float
       _stateElapsedTime,
       _stateMaxTime;

    public SlimeIdleState(SlimeBase owner) : base(owner)
    {
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
        if (_stateElapsedTime > _stateMaxTime)
        {
            _owner.SetState(new SlimeWanderState(_owner));
        }
        else
        {
            _stateElapsedTime += Time.deltaTime;
        }
    }
}
