using Godot;
using System.Collections.Generic;

public class Game : Node2D
{
	List<Player> PlayerList;

	Node2D PlayersNode;

	VBoxContainer PlayerScoreboard;

	public override void _Ready()
	{
		PlayerScoreboard = FindNode("PlayerList") as VBoxContainer;

		PlayersNode = FindNode("Players") as Node2D;
		PlayerList = new List<Player>();
		for(int x = 0; x < 12; x++)
		{
			var plyr = new Player();

			plyr.SetPlayerName("Player " + (x + 1));

			PlayersNode.AddChild(plyr);
			PlayerList.Add(plyr);

			var label = new Label();
			label.Text = plyr.PlayerName;
			PlayerScoreboard.AddChild(label);
		}
	}
}
