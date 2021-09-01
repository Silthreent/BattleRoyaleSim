using Godot;
using System.Collections.Generic;

public class KillActivity : BaseActivity
{
    public KillActivity()
    {
        R_MinEnemyPlayers = 1;
    }

    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var alive = interactable.FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
        if(alive.Count <= 0)
        {
            Game.CurrentGame.PostMessage($"{host.Name} was blood thirsty, but couldn't find anyone.");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];
        sacrifice.LoseHealth();

        Game.CurrentGame.PostMessage($"{host.Name} killed {sacrifice.Name}");

        GD.Print($"Killing {sacrifice.PlayerName}");

        return new List<Player>() { sacrifice };
    }
}
