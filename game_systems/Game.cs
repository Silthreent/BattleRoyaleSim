using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
	public static Game CurrentGame { get; protected set; }

	public static Random RNG;

	PackedScene PlayerScene;

	List<Player> PlayerList;

	VBoxContainer PlayerScoreboard;
	VBoxContainer MessageLog;
	ScrollContainer MessageScroll;

	GameState CurrentState;
	List<Player> UnprocessedPlayers;

	public Game()
	{
		CurrentGame = this;

		RNG = new Random();

		CurrentState = GameState.Day;
	}

	public override void _Ready()
	{
		PlayerScoreboard = FindNode("PlayerList") as VBoxContainer;
		MessageScroll = FindNode("MessageScroll") as ScrollContainer;
		MessageLog = MessageScroll.FindNode("MessageLog") as VBoxContainer;

		PlayerScene = ResourceLoader.Load<PackedScene>("res://game_systems/player/Player.tscn");
		PlayerList = new List<Player>();
		int teamCount = 1;
		int counter = 0;
		for(int x = 0; x < 12; x++)
		{
			var plyr = PlayerScene.Instance() as Player;

			plyr.SetPlayerName("Player " + (x + 1));
			plyr.Name = plyr.PlayerName;
			plyr.SetTeam(teamCount);
			plyr.MoveTo(Map.CurrentMap.GetLocale(teamCount - 1));

			PlayerList.Add(plyr);

			var label = new Label();
			label.Text = plyr.PlayerName + " - Team " + plyr.Team;
			PlayerScoreboard.AddChild(label);

			counter++;
			if(counter >= 2)
			{
				counter = 0;
				teamCount++;
			}
		}

		UnprocessedPlayers = new List<Player>(PlayerList);
	}

	public void PostMessage(string message)
	{
		var label = new Label();
		label.Text = message;
		label.Autowrap = true;
		MessageLog.AddChild(label);
		MessageLog.MoveChild(label, 0);
	}

	void ProcessRandomActivity()
	{
		GD.Print("--Processing day--");

		var chosenPlayer = UnprocessedPlayers[RNG.Next(0, UnprocessedPlayers.Count)];

		GD.Print($"Doing Activity: {chosenPlayer.PlayerName}");

		var activity = chosenPlayer.ChooseActivity();

		var involvedPlayers = activity.Process(chosenPlayer, chosenPlayer.CurrentLocale.GetLocalPlayers());
		foreach (var plyr in involvedPlayers)
		{
			UnprocessedPlayers.Remove(plyr);
		}
		UnprocessedPlayers.Remove(chosenPlayer);
	}

	void PostProcessActivities()
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
				label.Text = PlayerList[x].PlayerName + " - Team " + PlayerList[x].Team;
				PlayerScoreboard.AddChild(label);
			}
			else if (PlayerList[x].Health < 0)
			{
				PlayerList[x].CurrentLocale.RemovePlayer(PlayerList[x]);

				PlayerList.RemoveAt(x);
				x--;
			}
		}
		 
		Player winner = null;
		foreach(var x in PlayerList)
		{
			if (winner == null)
				winner = x;
			else if (winner.Team != x.Team)
			{
				winner = null;
				break;
			}
		}

		if (winner != null)
			GameOver(winner);
	}

	void GameOver(Player player)
	{
		if (player != null)
			GD.Print($"TEAM {player.Team} WINS!!!!!!!");
		else
			GD.Print("EVERYBODY DIED!! BETTER LUCK NEXT TIME!!!!");
	}

	void OnNextButtonPressed()
	{
		switch(CurrentState)
		{
			case GameState.Day:
				ProcessRandomActivity();

				if (UnprocessedPlayers.Count <= 0)
					CurrentState = GameState.EndDay;
				break;

			case GameState.EndDay:
				PostProcessActivities();

				UnprocessedPlayers = new List<Player>(PlayerList);
				CurrentState = GameState.Night;
				break;

			case GameState.Night:
				ProcessRandomActivity();

				if (UnprocessedPlayers.Count <= 0)
					CurrentState = GameState.EndNight;
				break;

			case GameState.EndNight:
				PostProcessActivities();

				UnprocessedPlayers = new List<Player>(PlayerList);
				CurrentState = GameState.Day;
				break;
		}
	}
}

enum GameState
{
	Day,
	EndDay,
	Night,
	EndNight,
	GameOver
}
