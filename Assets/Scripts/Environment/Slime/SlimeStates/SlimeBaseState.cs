public abstract class SlimeBaseState{

    protected SlimeBase _owner;

    public abstract void Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public SlimeBaseState(SlimeBase owner)
    {
        _owner = owner;
    }
}
