public class ExplosiveItem : BaseItem
{
    public ExplosiveItem()
    {
        Name = "Explosive";

        PossiblesActs = new BaseActivity[]
        {
            ActivityList.GetActivity<UseExplosiveActivity>()
        };
    }
}
