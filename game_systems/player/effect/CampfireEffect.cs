public class CampfireEffect : BaseEffect
{
    MapLocale CurrentLocale;

    public CampfireEffect(MapLocale host)
    {
        CurrentLocale = host;
    }

    public override void TickEffect()
    {
        CurrentLocale.LoseEffect(typeof(CampfireEffect));
    }
}
