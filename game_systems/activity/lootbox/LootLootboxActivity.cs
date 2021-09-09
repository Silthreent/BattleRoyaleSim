using System.Collections.Generic;

[Activity("lootbox", "opener")]
public class LootLootboxActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        if (host.CurrentLocale.HasEffect(typeof(LootboxEffect)))
            return true;

        return false;
    }

    public override List<Player> Process(Player host)
    {
        PostMessage($"{host.PlayerName} tried to loot The Lootbox but that isn't implemented yet...");

        return base.Process(host);
    }

}
