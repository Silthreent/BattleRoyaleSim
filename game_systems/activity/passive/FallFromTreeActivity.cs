using System.Collections.Generic;

[Activity("tree")]
public class FallFromTreeActivity : BaseActivity
{
    // Fall out of the tree and die

    public override bool CanProcess(Player host)
    {
        return host.HasEffect(typeof(InTreeEffect));
    }

    public override List<Player> Process(Player host)
    {
        PostMessage($"{host.PlayerName} is chillin' in a tree.");

        return base.Process(host);
    }
}
