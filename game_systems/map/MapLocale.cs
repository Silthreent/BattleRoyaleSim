using Godot;
using System.Collections.Generic;

public class MapLocale : GridContainer
{
	[Export]
	public int[] Connections;

	public List<MapLocale> ConnectedLocale { get; protected set; }

	List<Player> LocalPlayers;

	public MapLocale()
	{
		ConnectedLocale = new List<MapLocale>();
		LocalPlayers = new List<Player>();
	}

	public void AddPlayer(Player player)
	{
		var control = new Control();
		AddChild(control);

		if(player.GetParent() != null)
			player.GetParent().RemoveChild(player);

		control.AddChild(player);
		player.Position = new Vector2(0, 0);
		control.RectMinSize = player.GetSpriteSize();

		LocalPlayers.Add(player);
	}

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
}
