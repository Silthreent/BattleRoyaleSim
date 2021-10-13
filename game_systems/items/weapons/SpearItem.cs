public class SpearItem : BaseItem
{
    public SpearItem()
    {
        Name = "Spear";

        PossiblesActs = new BaseActivity[]
        {
            ActivityList.GetActivity<SpearStabActivity>()
        };
    }

    public override int ModifyAttackRoll(int roll)
    {
        return 250;
    }
}
