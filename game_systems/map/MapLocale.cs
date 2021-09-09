using Godot;
using System;
using System.Collections.Generic;

public class MapLocale : GridContainer
{
	[Export]
	public int[] Connections;

	public List<MapLocale> ConnectedLocale { get; protected set; }

	List<Player> LocalPlayers;

	List<BaseEffect> Effects;

	public MapLocale()
	{
		ConnectedLocale = new List<MapLocale>();
		LocalPlayers = new List<Player>();

		Effects = new List<BaseEffect>();
	}

	/// <summary>
	/// Move a player to this locale. Keep track of it here, as well as visually placing it.
	/// </summary>
	/// <param name="player">The player to move here.</param>
	public void AddPlayer(Player player)
	{
		// Positions the player by placing them underneath a Control in the GridContainer
		// The player is a Node2D, but will still be moved by the Control which is being moved by the GridContainer

		// Create the control to house the player
		var control = new Control();
		AddChild(control);

		if(player.GetParent() != null)
			player.GetParent().RemoveChild(player);

		// Add the player to the control, move them to center, and resize the control to match the player size
		control.AddChild(player);
		player.Position = new Vector2(0, 0);
		control.RectMinSize = player.GetSpriteSize();

		LocalPlayers.Add(player);
	}

	// Everything add does, but in reverse
	// So not really the same, it just gets rid of them from here
	public void RemovePlayer(Player player)
	{
		var parent = player.GetParent();

		if(parent != null)
			parent.RemoveChild(player);

		parent.QueueFree();

		LocalPlayers.Remove(player);
	}

	public List<Player> GetLocalPlayers()
	{
		return new List<Player>(LocalPlayers);
	}

	public int GetLocalPlayerCount()
    {
		return LocalPlayers.Count;
    }

	public void ModifyActivityList(List<BaseActivity> possibles)
    {
		int count = possibles.Count;
		for (int x = 0; x < possibles.Count; x++)
		{
			foreach (var effect in Effects)
			{
				if (!effect.CanDoActivity(possibles[x]))
					possibles.RemoveAt(x);

				if (possibles.Count < count)
					x -= count - possibles.Count;

				count = possibles.Count;
			}
		}
	}

	public void GiveEffect(BaseEffect effect)
	{
		Effects.Add(effect);
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

	public void ProcessTimeChange()
	{
		int count = Effects.Count;
		for (int x = 0; x < Effects.Count; x++)
		{
			Effects[x].TickEffect();

			if (Effects.Count < count)
				x -= count - Effects.Count;

			count = Effects.Count;
		}
	}
}
