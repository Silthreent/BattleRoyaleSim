public class SwordItem : BaseItem
{
    public SwordItem()
    {
        Name = "Sword";
    }

    public override int ModifyAttackRoll(int roll)
    {
        return 350;
    }
}
