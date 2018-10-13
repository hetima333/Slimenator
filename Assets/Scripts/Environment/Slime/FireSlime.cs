public class FireSlime : SlimeBase
{
    // Use this for initialization
    protected override void Start ()
    {
        Init(100f, 5.0f, SlimeStats.Slime_Type.SLIME_FIRE);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}
}
