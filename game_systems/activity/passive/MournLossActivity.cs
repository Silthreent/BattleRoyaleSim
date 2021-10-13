using System.Collections.Generic;

[Activity]
public class MournLossActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        if (Game.CurrentGame.GetPlayers().Find(x => x.Team == host.Team && x.Health <= 0) != null)
            return true;

        return false;
    }

    public override List<Player> Process(Player host)
    {
        var deadTeam = Game.CurrentGame.GetPlayers().FindAll(x => x.Team == host.Team && x.Health <= 0);

        int index = 0;
        if (deadTeam.Count != 1)
            index = Game.RNG.Next(0, deadTeam.Count);

        PostMessage($"{host.PlayerName} mourns the loss of their teammate, {deadTeam[index].PlayerName}.");

        return base.Process(host);
    }
}
