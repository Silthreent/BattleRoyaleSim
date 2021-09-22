public class BowNArrowItem : BaseItem
{
    public BowNArrowItem()
    {
        Name = "Bow";

        PossiblesActs = new BaseActivity[]
        {
            ActivityList.GetActivity<ShootBowActivity>()
        };
    }

    public override int ModifyAttackRoll(int roll)
    {
        return 150;
    }
}
