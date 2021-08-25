using Godot;
using System.Collections.Generic;

public class MapLocale : GridContainer
{
	[Export]
	public int[] Connections;

	public List<MapLocale> ConnectedLocale { get; protected set; } = new List<MapLocale>();

	public void AddPlayer(Player player)
	{
		var control = new Control();
		AddChild(control);

		if(player.GetParent() != null)
			player.GetParent().RemoveChild(player);

		control.AddChild(player);
		player.Position = new Vector2(0, 0);
	}

	public void RemovePlayer(Player player)
	{
		var parent = player.GetParent();

		parent.RemoveChild(player);
		parent.QueueFree();
	}
}
