using System.Collections.Generic;

public class LootboxEffect : BaseEffect
{
    List<BaseItem> Inventory;

    public LootboxEffect()
    {
        Inventory = new List<BaseItem>()
        {
            new SpearItem(),
            new SpearItem(),
            new SpearItem(),

            new BowNArrowItem(),
            new BowNArrowItem(),

            new SwordItem()
        };
    }

    public BaseItem GrabItem()
    {
        if (Inventory.Count <= 0)
            return null;

        var itemIndex = Game.RNG.Next(0, Inventory.Count);
        var item = Inventory[itemIndex];
        Inventory.RemoveAt(itemIndex);

        return item;
    }
}
