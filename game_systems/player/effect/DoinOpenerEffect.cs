public class DoinOpenerEffect : BaseEffect
{
    public override bool CanDoActivity(BaseActivity activity)
    {
        return CheckForTag(activity, "opener");
    }

    public override void TickEffect()
    {
        Host.LoseEffect(this.GetType());
    }
}
