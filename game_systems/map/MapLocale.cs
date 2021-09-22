using Godot;
using System;
using System.Collections.Generic;

public class MapLocale : GridContainer
{
	[Export]
	public int[] Connections;

	public List<MapLocale> ConnectedLocale { get; protected set; }
	public EntityData Entity { get; protected set; }

	List<Player> LocalPlayers;

	public MapLocale()
	{
		ConnectedLocale = new List<MapLocale>();
		LocalPlayers = new List<Player>();

		Entity = new EntityData();
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
		Entity.ModifyActivityList(possibles);
	}

	public void ProcessTimeChange()
	{
		Entity.ProcessTimeChange();
	}
}
