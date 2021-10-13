using System.Collections.Generic;

[Activity]
public class SitAtCampfireActivity : BaseActivity
{
	public override bool CanProcess(Player host)
	{
		return host.CurrentLocale.Entity.HasEffect<CampfireEffect>();
	}

	public override List<Player> Process(Player host)
	{
		PostMessage($"{host.PlayerName} joins the campfire sing-a-longs.");

		return base.Process(host);
	}
}
