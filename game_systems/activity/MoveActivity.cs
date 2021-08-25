using System.Collections.Generic;

public class MoveActivity : BaseActivity
{
    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var currentLocale = host.CurrentLocale;
        host.MoveTo(currentLocale.ConnectedLocale[Game.RNG.Next(0, currentLocale.ConnectedLocale.Count)]);

        return new List<Player>();
    }
}
