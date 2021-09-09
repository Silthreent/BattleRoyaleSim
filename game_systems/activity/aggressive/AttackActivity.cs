using System.Collections.Generic;

[Activity]
public class AttackActivity : BaseActivity
{
    // Player attempts to damage a non-teammate at the same locale
    // Can backfire, causing them to take damage instead

    public override bool CanProcess(Player host)
    {
        return HasNearbyEnemies(host, 1);
    }

    public override List<Player> Process(Player host)
    {
        var alive = host.CurrentLocale.GetLocalPlayers().FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
        if (alive.Count <= 0)
        {
            Game.CurrentGame.PostMessage($"{host.Name} was blood thirsty, but couldn't find anyone.");

            return new List<Player>();
        }

        var sacrifice = alive[Game.RNG.Next(0, alive.Count)];

        var hRoll = host.RollAttack();
        var tRoll = sacrifice.RollAttack();

        if(hRoll >= tRoll)
        {
            Game.CurrentGame.PostMessage($"{host.Name} attacks and kills {sacrifice.Name}.");
            sacrifice.LoseHealth();
        }
        else
        {
            Game.CurrentGame.PostMessage($"{host.Name} tried to attack {sacrifice.Name}, but they parried it.");
            host.LoseHealth();
        }

        return new List<Player>() { sacrifice };
    }
}
