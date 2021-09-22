using System.Collections.Generic;

[Activity("lootbox", "opener")]
public class LootLootboxActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        if (host.CurrentLocale.Entity.HasEffect(typeof(LootboxEffect)))
            return true;

        return false;
    }

    public override List<Player> Process(Player host)
    {
        var item = host.CurrentLocale.Entity.GetEffect<LootboxEffect>().GrabItem();

        if(item != null)
        {
            host.Entity.GiveItem(item);
            PostMessage($"{host.PlayerName} grabbed a {item.Name} from The Lootbox!");
        }
        else
        {
            PostMessage($"{host.PlayerName} tried to loot The Lootbox, but found it empty...");
        }

        return base.Process(host);
    }

}
