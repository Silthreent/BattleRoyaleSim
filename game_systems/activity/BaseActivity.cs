using System;
using System.Collections.Generic;

public abstract class BaseActivity
{
    public abstract bool CanProcess(Player host);

    /// <summary>
    /// Process the activity doing everything it says to.
    /// </summary>
    /// <param name="host">The player doing the activity</param>
    /// <returns>Which players were involved</returns>
    public virtual List<Player> Process(Player host)
    {
        return new List<Player>();
    }

    protected bool HasNearbyEnemies(Player host, int amount)
    {
        if(host.CurrentLocale.GetLocalAlive().FindAll(x => x.Team != host.Team).Count >= amount)
            return true;

        return false;
    }

    protected void PostMessage(string message)
    {
        Game.CurrentGame.PostMessage(message);
    }
}

class ActivityAttribute : Attribute
{
    public string[] Tags;

    public ActivityAttribute(params string[] args)
    {
        Tags = args;
    }

    public bool HasTag(string tag)
    {
        return Array.Exists(Tags, x => x == tag);
    }
}
