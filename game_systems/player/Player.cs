using Godot;

public class Player : Node2D
{
	public string PlayerName { get; protected set; }
	public int Team { get; protected set; }
	public int Health { get; protected set; }
	public MapLocale CurrentLocale { get; protected set; }

	public BaseActivity ChooseActivity()
	{
		return new KillActivity();
	}

	public void MoveTo(MapLocale locale)
	{
		Map.CurrentMap.MovePlayer(this, locale);
	}

	public void SetPlayerName(string name)
	{
		PlayerName = name;
	}

	public void SetTeam(int team)
	{
		Team = team;
	}

	public void LoseHealth()
	{
		Health--;
	}
}
