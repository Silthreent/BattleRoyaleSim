using System.Collections.Generic;
using System.Linq;

[Activity]
public class LootAreaActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        if (host.CurrentLocale.Entity.Inventory.Count > 0)
            return true;

        if (host.CurrentLocale.GetLocalPlayers().Where(x => x.Health <= 0).Where(x => x.Entity.Inventory.Count > 0).Count() > 0)
            return true;

        return false;
    }

    public override List<Player> Process(Player host)
    {
        var itemChoices = new List<(BaseItem, EntityData)>();
        foreach(var item in host.CurrentLocale.Entity.Inventory)
        {
            itemChoices.Add((item, host.CurrentLocale.Entity));
        }
        foreach(var player in host.CurrentLocale.GetLocalPlayers().Where(x => x.Health <= 0).Where(x => x.Entity.Inventory.Count > 0))
        {
            foreach(var item in player.Entity.Inventory)
            {
                itemChoices.Add((item, player.Entity));
            }
        }

        if(itemChoices.Count <= 0)
        {
            PostMessage($"{host.PlayerName} tried to take an item from nearby, but couldn't find anything...");
            return base.Process(host);
        }

        var stolenItem = itemChoices[Game.RNG.Next(0, itemChoices.Count)];

        if (stolenItem.Item2.HostPlayer != null)
            PostMessage($"{host.PlayerName} picked a {stolenItem.Item1.Name} off of {stolenItem.Item2.HostPlayer.PlayerName}'s corpse.");
        else if (stolenItem.Item2.HostLocale != null)
            PostMessage($"{host.PlayerName} picked up a {stolenItem.Item1.Name} off the ground.");

        stolenItem.Item2.LoseItem(stolenItem.Item1);
        host.Entity.GiveItem(stolenItem.Item1);

        return base.Process(host);
    }
}
