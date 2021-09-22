using System.Reflection;

public class BaseEffect
{
    public virtual bool CanDoActivity(BaseActivity activity) { return true; }
    public virtual void TickEffect(EntityData entity) { }

    protected bool CheckForTag(BaseActivity activity, string tag)
    {
        return activity.GetType().GetCustomAttribute<ActivityAttribute>().HasTag(tag);
    }
}
