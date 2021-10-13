using System.Collections.Generic;

[Activity("item")]
public class SpearStabActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        return HasNearbyEnemies(host, 1);
    }

    public override List<Player> Process(Player host)
    {
        var alive = host.CurrentLocale.GetLocalPlayers().FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
        if (alive.Count <= 0)
        {
            Game.CurrentGame.PostMessage($"{host.PlayerName} looked to stab with their spear, but couldn't find anyone.");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];

        var hRoll = host.RollAttack();
        var tRoll = sacrifice.RollAttack();

        if (hRoll >= tRoll)
        {
            Game.CurrentGame.PostMessage($"{host.Name} lunges forward with their spear and impales {sacrifice.Name}.");
            sacrifice.LoseHealth();
        }
        else
        {
            Game.CurrentGame.PostMessage($"{host.Name} tried to stab {sacrifice.Name} with their spear but missed.");
        }

        return new List<Player>() { sacrifice };
    }
}
