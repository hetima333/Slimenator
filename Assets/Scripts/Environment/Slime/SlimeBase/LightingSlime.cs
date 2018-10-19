public class LightingSlime : SlimeBase
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Init(float maxHealth, float velocity, ElementType type)
    {
        base.Init(maxHealth, velocity * 2, type);
    }
}