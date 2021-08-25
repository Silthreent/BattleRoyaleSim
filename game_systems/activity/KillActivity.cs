using Godot;
using System.Collections.Generic;

public class KillActivity : BaseActivity
{
    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var alive = interactable.FindAll(x => x.Health >= 0 && x != host);
        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];
        sacrifice.LoseHealth();

        GD.Print($"Killing {sacrifice.PlayerName}");

        return new List<Player>() { sacrifice };
    }
}
