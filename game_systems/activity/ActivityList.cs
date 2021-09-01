using System;
using System.Collections.Generic;
using System.Linq;

public static class ActivityList
{
    public static List<BaseActivity> Activities;

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

    public static BaseActivity[] GetPossibleActivities(Player player)
    {
        var localePlayers = player.CurrentLocale.GetLocalPlayers();
        var enemyPlayers = localePlayers.Where(x => x.Team != player.Team);

        return Activities
            .Where(x => x.R_MinEnemyPlayers <= enemyPlayers.Count() && x.R_MaxEnemyPlayers >= enemyPlayers.Count())
            .ToArray();
    }
}
