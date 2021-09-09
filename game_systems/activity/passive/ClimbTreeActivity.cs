using System.Collections.Generic;

[Activity]
public class ClimbTreeActivity : BaseActivity
{
    // Player climbs a tree to try and be safe

    public override bool CanProcess(Player host)
    {
        return true;
    }

    public override List<Player> Process(Player host)
    {
        host.GiveEffect(new InTreeEffect());

        PostMessage($"{host.PlayerName} climbed a tree.");

        return new List<Player>();
    }
}
