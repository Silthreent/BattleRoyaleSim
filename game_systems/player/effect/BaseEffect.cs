using System.Reflection;

public class BaseEffect
{
    public Player Host { get; set; }

    public virtual bool CanDoActivity(BaseActivity activity) { return true; }

    protected bool CheckForTag(BaseActivity activity, string tag)
    {
        return activity.GetType().GetCustomAttribute<ActivityAttribute>().HasTag(tag);
    }
}
