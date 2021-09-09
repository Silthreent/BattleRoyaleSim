using System.Collections.Generic;

public class NoOptionActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        return false;
    }

    public override List<Player> Process(Player host)
    {
        PostMessage($"{host.PlayerName} is frolicking in the flowers...");

        return new List<Player>();
    }
}
