using Godot;
using System;
using System.Collections.Generic;

public class Player : Node2D
{
	[Signal]
	public delegate void PlayerDied();

	public string PlayerName { get; protected set; }
	public int Team { get; protected set; }
	public int Health { get; protected set; }
	public MapLocale CurrentLocale { get; protected set; }

	Sprite Sprite;

	List<BaseEffect> Effects;

	public Player()
	{
		Effects = new List<BaseEffect>();
	}

	public override void _Ready()
	{
		Sprite = GetNode<Sprite>("Sprite");
	}

	public BaseActivity ChooseActivity()
	{
		var possibles = ActivityList.GetPossibleActivities(this);

		int count = possibles.Count;
		for(int x = 0; x < possibles.Count; x++)
        {
			foreach (var effect in Effects)
			{
				if (!effect.CanDoActivity(possibles[x]))
					possibles.RemoveAt(x);

				if(possibles.Count < count)
					x -= count - possibles.Count;

				count = possibles.Count;
			}
		}

		return possibles[Game.RNG.Next(0, possibles.Count)];
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

	// TODO: Update to wound system
	public void LoseHealth()
	{
		Health--;

		if(Health <= 0)
		{
			EmitSignal("PlayerDied");
		}
	}

	public void GiveEffect(BaseEffect effect)
	{
		Effects.Add(effect);
		effect.Host = this;
	}

	public void LoseEffect(Type eType)
    {
		var effect = Effects.Find(x => x.GetType() == eType);
		if (effect != null)
			Effects.Remove(effect);
    }

	public bool HasEffect(Type effectType)
	{
		return Effects.Find(x => x.GetType() == effectType) != null;
	}

	public Vector2 GetSpriteSize()
	{
		return Sprite.Texture.GetSize() * Sprite.Scale;
	}
}
