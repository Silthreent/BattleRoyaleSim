using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
	public static Random RNG;

	List<Player> PlayerList;

	Node2D PlayersNode;

	VBoxContainer PlayerScoreboard;

	public Game()
	{
		RNG = new Random();
	}

	public override void _Ready()
	{
		PlayerScoreboard = FindNode("PlayerList") as VBoxContainer;

		PlayersNode = FindNode("Players") as Node2D;
		PlayerList = new List<Player>();
		for(int x = 0; x < 12; x++)
		{
			var plyr = new Player();

			plyr.SetPlayerName("Player " + (x + 1));
			plyr.Name = plyr.PlayerName;

			PlayersNode.AddChild(plyr);
			PlayerList.Add(plyr);

			var label = new Label();
			label.Text = plyr.PlayerName;
			PlayerScoreboard.AddChild(label);
		}
	}

	void ProcessDay()
	{
		GD.Print("Processing day");

		var activePlayers = new List<Player>(PlayerList);
		while(activePlayers.Count > 0)
		{
			var chosenPlayer = activePlayers[RNG.Next(0, activePlayers.Count)];

			GD.Print($"Doing Activity: {chosenPlayer.PlayerName}");

			var activity = chosenPlayer.ChooseActivity();

			var involvedPlayers = activity.Process(chosenPlayer, PlayerList);
			foreach(var plyr in involvedPlayers)
			{
				activePlayers.Remove(plyr);
			}
			activePlayers.Remove(chosenPlayer);
		}

		PostProcessDay();
	}

	void PostProcessDay()
	{
		foreach(Node x in PlayerScoreboard.GetChildren())
		{
			x.QueueFree();
		}

		for(int x = 0; x < PlayerList.Count; x++)
        {
			if (PlayerList[x].Health >= 0)
			{
				var label = new Label();
				label.Text = PlayerList[x].PlayerName;
				PlayerScoreboard.AddChild(label);
			}
			else if (PlayerList[x].Health < 0)
			{
				PlayerList.RemoveAt(x);

				x--;
			}
		}
	}

	void OnNextButtonPressed()
	{
		ProcessDay();
	}
}
