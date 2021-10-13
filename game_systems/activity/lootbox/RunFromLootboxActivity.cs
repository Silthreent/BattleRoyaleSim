using System.Collections.Generic;

[Activity("opener")]
public class RunFromLootboxActivity : BaseActivity
{
	public override bool CanProcess(Player host)
	{
		if (host.Entity.HasEffect<DoinOpenerEffect>())
			return true;

		return false;
	}

	public override List<Player> Process(Player host)
	{
		var currentLocale = host.CurrentLocale;
		host.MoveTo(currentLocale.ConnectedLocale[Game.RNG.Next(0, currentLocale.ConnectedLocale.Count)]);

		PostMessage($"{host.Name} ran from The Lootbox to {host.CurrentLocale.Name}");

		return new List<Player>();
	}
}
