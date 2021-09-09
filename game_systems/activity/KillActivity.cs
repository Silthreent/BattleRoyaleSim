using Godot;
using System.Collections.Generic;

public class KillActivity : BaseActivity
{
    // Player kills a non-teammate at the same locale

    public override bool CanProcess(Player host)
    {
        return HasNearbyEnemies(host, 1);
    }

    public override List<Player> Process(Player host)
    {
        var alive = host.CurrentLocale.GetLocalPlayers().FindAll(x => x.Team != host.Team);
        if(alive.Count <= 0)
        {
            PostMessage($"{host.Name} was blood thirsty, but couldn't find anyone.");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];
        sacrifice.LoseHealth();

        PostMessage($"{host.Name} killed {sacrifice.Name}");

        GD.Print($"Killing {sacrifice.PlayerName}");

        return new List<Player>() { sacrifice };
    }
}
