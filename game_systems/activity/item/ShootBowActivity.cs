using System.Collections.Generic;

[Activity("item")]
public class ShootBowActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        return HasNearbyEnemies(host, 1);
    }

    public override List<Player> Process(Player host)
    {
        var alive = host.CurrentLocale.GetLocalAlive().FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
        if (alive.Count <= 0)
        {
            Game.CurrentGame.PostMessage($"{host.PlayerName} looking to shoot an arrow, but couldn't find anyone.");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];

        var attemptRoll = Game.RNG.Next(0, 1000);

        if (attemptRoll <= 800)
        {
            Game.CurrentGame.PostMessage($"{host.PlayerName} lets fly an arrow and it pierces {sacrifice.PlayerName}.");
            sacrifice.LoseHealth();
        }
        else
        {
            Game.CurrentGame.PostMessage($"{host.PlayerName} lets fly an arrow at {sacrifice.PlayerName}, but it was a little to the left.");
        }

        return new List<Player>() { sacrifice };
    }
}
