using System;
using System.Collections.Generic;
using System.Linq;

public static class ActivityList
{
    public static List<BaseActivity> Activities;

    // Loads every activity possible for use by players
    static ActivityList()
    {
        var acts = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(x => x.GetTypes())
        .Where(x => x.IsClass)
        .Where(x => x.GetCustomAttributes(typeof(ActivityAttribute), false).FirstOrDefault() != null)
        .ToArray();

        Activities = new List<BaseActivity>();
        foreach(var x in acts)
        {
            Activities.Add(Activator.CreateInstance(x) as BaseActivity);
        }
    }

    /// <summary>
    /// Get every activity the player could do right now.
    /// Based on how many nearby players, nearby allies, and current tags and effects.
    /// </summary>
    /// <param name="player">Player that wants to do an activity</param>
    /// <returns>All possible activities.</returns>
    public static List<BaseActivity> GetPossibleActivities(Player player)
    {
        return Activities
            .Where(x => x.CanProcess(player))
            .ToList();
    }
}
