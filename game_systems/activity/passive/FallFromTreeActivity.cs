using System.Collections.Generic;

[Activity("tree")]
public class FallFromTreeActivity : BaseActivity
{
    // Fall out of the tree and die

    public override bool CanProcess(Player host)
    {
        return host.Entity.HasEffect(typeof(InTreeEffect));
    }

    public override List<Player> Process(Player host)
    {
        host.LoseHealth();

        PostMessage($"{host.PlayerName} fell out of the tree and died.");

        return base.Process(host);
    }
}
