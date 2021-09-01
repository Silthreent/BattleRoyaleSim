using System.Collections.Generic;

[Activity]
public class MoveActivity : BaseActivity
{
    public override List<Player> Process(Player host, List<Player> interactable)
    {
        var currentLocale = host.CurrentLocale;
        host.MoveTo(currentLocale.ConnectedLocale[Game.RNG.Next(0, currentLocale.ConnectedLocale.Count)]);

        Game.CurrentGame.PostMessage($"{host.Name} moved to {host.CurrentLocale.Name}");

        return new List<Player>();
    }
}
