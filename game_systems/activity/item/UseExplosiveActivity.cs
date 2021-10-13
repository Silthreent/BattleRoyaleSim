using Godot;
using System.Collections.Generic;
using System.Linq;

[Activity("item")]
public class UseExplosiveActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        var localAlive = host.CurrentLocale.GetLocalAlive();
        var nearbyEnemies = localAlive.Where(x => x.Team != host.Team).Count();
        var nearbyAllies = localAlive.Where(x => x.Team == host.Team).Count();

        GD.Print($"Nearby Enemies: {nearbyEnemies}");
        GD.Print($"Nearby Allies: {nearbyAllies}");

        return nearbyEnemies >= nearbyAllies;
    }

    public override List<Player> Process(Player host)
    {
        PostMessage($"{host.PlayerName} sets off their explosive!");

        var alive = host.CurrentLocale.GetLocalAlive();

        foreach(var player in alive)
        {
            var roll = Game.RNG.Next(0, 1000);
            GD.Print($"Explosion Roll: {roll}");
            if (player == host)
            {
                if (roll <= 100)
                {
                    player.LoseHealth();
                    PostMessage($"{player.PlayerName} is caught in the explosion.");
                }
                else
                    PostMessage($"{player.PlayerName} manages to dive out of the way.");
            }
            else if (player.Team == host.Team)
            {
                if (roll <= 200)
                {
                    player.LoseHealth();
                    PostMessage($"{player.PlayerName} is caught in the explosion.");
                }
                else
                    PostMessage($"{player.PlayerName} manages to dive out of the way.");
            }
            else
            {
                if (roll <= 650)
                {
                    player.LoseHealth();
                    PostMessage($"{player.PlayerName} is caught in the explosion.");
                }
                else
                    PostMessage($"{player.PlayerName} manages to dive out of the way.");
            }
        }

        host.Entity.LoseItem<ExplosiveItem>();

        return new List<Player>(alive);
    }
}
