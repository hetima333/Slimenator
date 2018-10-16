using UnityEngine;

public class SlimeWanderState : SlimeBaseState
{
    Vector3
       _startPosition,
       _destination;

    float
       _stateElapsedTime,
       _stateMaxTime;

    public SlimeWanderState(SlimeBase owner) : base(owner)
    {
        _startPosition = _owner.GetPosition();
    }

    public override void OnStateEnter()
    {
        _destination = GetNewDestination();
        _stateElapsedTime = 0;
        _stateMaxTime = UnityEngine.Random.Range(3.0f, 5.0f);
    }

    public override void OnStateExit()
    {
    }

    public override void Tick()
    {
        // ENG: Get new destination, once it reached current destination
        // JAP: 現在の目的地に到着したら、新しい目的地を取得する
        if (ReachedDestination())
        {
            _destination = GetNewDestination();
        }

        _owner.MoveToward(_destination);

        if (_stateElapsedTime > _stateMaxTime)
        {
            _owner.SetState(new SlimeIdleState(_owner));
        }
        else
        {
            _stateElapsedTime += Time.deltaTime;
        }
    }

    private Vector3 GetNewDestination()
    {
        Vector3 newDestination = new Vector3(UnityEngine.Random.Range(_owner.GetPosition().x - _owner.Stats.MovementRange, _owner.GetPosition().x + _owner.Stats.MovementRange), _owner.GetPosition().y, UnityEngine.Random.Range(_owner.GetPosition().z - -_owner.Stats.MovementRange, _owner.GetPosition().z + -_owner.Stats.MovementRange));

        // ENG: Ensure that it moves at a certain range.
        // JAP: 特定の範囲で移動することを確認する.
        if (newDestination.x > _startPosition.x + _owner.Stats.MaxMovementRange || newDestination.z > _startPosition.z + _owner.Stats.MaxMovementRange ||
            newDestination.x < _startPosition.x - _owner.Stats.MaxMovementRange || newDestination.z < _startPosition.z - _owner.Stats.MaxMovementRange)
        {
            return GetNewDestination();
        }

        return newDestination;
    }

    private bool ReachedDestination()
    {
        return Vector3.Distance(_owner.GetPosition(), _destination) < 0.5f;
    }
}
