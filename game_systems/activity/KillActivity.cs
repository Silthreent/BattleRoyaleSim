using Godot;
using System.Collections.Generic;

public class KillActivity : BaseActivity
{
    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var alive = interactable.FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
        if(alive.Count <= 0)
        {
            GD.Print("Failed to find target to kill");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];
        sacrifice.LoseHealth();

        GD.Print($"Killing {sacrifice.PlayerName}");

        return new List<Player>() { sacrifice };
    }
}
