public class InTreeEffect : BaseEffect
{
    public override bool CanDoActivity(BaseActivity activity)
    {
        return CheckForTag(activity, "tree");
    }
}
