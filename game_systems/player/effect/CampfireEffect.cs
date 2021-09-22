public class CampfireEffect : BaseEffect
{
    public override void TickEffect(EntityData entity)
    {
        entity.LoseEffect(typeof(CampfireEffect));
    }
}
