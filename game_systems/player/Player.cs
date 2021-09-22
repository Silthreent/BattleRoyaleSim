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
	public EntityData Entity { get; protected set; }
	public MapLocale CurrentLocale { get; protected set; }

	Sprite Sprite;

	public Player()
    {
		Entity = new EntityData();
    }

	public override void _Ready()
	{
		Sprite = GetNode<Sprite>("Sprite");
	}

	public void ProcessTimeChange()
    {
		Entity.ProcessTimeChange();
	}

	public BaseActivity ChooseActivity()
	{
		var possibles = ActivityList.GetPossibleActivities(this);

		Entity.ModifyActivityList(possibles);
		CurrentLocale.ModifyActivityList(possibles);

		if (possibles.Count <= 0)
			return new NoOptionActivity();

		return possibles[Game.RNG.Next(0, possibles.Count)];
	}

	public void MoveTo(MapLocale locale)
	{
		Map.CurrentMap.MovePlayer(this, locale);
		CurrentLocale = locale;
	}

	public int RollAttack()
	{
		var attackRoll = Game.RNG.Next(0, 1000);

		var modifier = 0;
		Entity.ProcessOnInventory(x => { modifier += x.ModifyAttackRoll(attackRoll); });

		GD.Print($"{PlayerName} rolling for attack");
		GD.Print($"Attack roll: {attackRoll}");
		GD.Print($"Modifier: {modifier}");
		GD.Print($"Total: {attackRoll + modifier}");

		return attackRoll + modifier;
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

	public Vector2 GetSpriteSize()
	{
		return Sprite.Texture.GetSize() * Sprite.Scale;
	}
}
