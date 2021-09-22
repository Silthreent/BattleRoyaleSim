using System.Collections.Generic;

[Activity("tree")]
public class ClimbDownTreeActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        return host.Entity.HasEffect(typeof(InTreeEffect));
    }

    public override List<Player> Process(Player host)
    {
        host.Entity.LoseEffect(typeof(InTreeEffect));

        PostMessage($"{host.PlayerName} climbed down from the tree.");

        return new List<Player>();
    }
}
