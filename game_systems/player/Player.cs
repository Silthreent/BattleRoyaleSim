using Godot;

public class Player : Node2D
{
	[Signal]
	public delegate void PlayerDied();

	public string PlayerName { get; protected set; }
	public int Team { get; protected set; }
	public int Health { get; protected set; }
	public MapLocale CurrentLocale { get; protected set; }

	Sprite Sprite;

	public override void _Ready()
	{
		Sprite = GetNode<Sprite>("Sprite");
	}

	public BaseActivity ChooseActivity()
	{
		var possibles = ActivityList.GetPossibleActivities(this);

		return possibles[Game.RNG.Next(0, possibles.Length)];
	}

	public void MoveTo(MapLocale locale)
	{
		Map.CurrentMap.MovePlayer(this, locale);
		CurrentLocale = locale;
	}

	public int RollAttack()
	{
		return Game.RNG.Next(0, 1000);
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

		if(Health <= 0)
		{
			EmitSignal("PlayerDied");
		}
	}

	public Vector2 GetSpriteSize()
	{
		return Sprite.Texture.GetSize() * Sprite.Scale;
	}
}
