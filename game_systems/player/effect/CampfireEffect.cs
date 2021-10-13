public class CampfireEffect : BaseEffect
{
	public Player Host;

	public CampfireEffect(Player host)
	{
		Host = host;
	}

	public override void TickEffect(EntityData entity)
	{
		entity.LoseEffect<CampfireEffect>();
	}
}
