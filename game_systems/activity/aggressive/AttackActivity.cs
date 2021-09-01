using System.Collections.Generic;

[Activity]
public class AttackActivity : BaseActivity
{
    public AttackActivity()
    {
        R_MinEnemyPlayers = 1;
    }

    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var alive = interactable.FindAll(x => x.Health >= 0 && x != host && x.Team != host.Team);
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

        return base.Process(host, interactable);
    }
}
