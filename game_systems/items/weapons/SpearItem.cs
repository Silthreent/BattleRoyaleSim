public class SpearItem : BaseItem
{
    public SpearItem()
    {
        Name = "Spear";
    }

    public override int ModifyAttackRoll(int roll)
    {
        return 300;
    }
}
