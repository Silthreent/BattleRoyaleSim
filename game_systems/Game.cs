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

	List<Player> DayDeathList;

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
			plyr.Connect("PlayerDied", this, "OnPlayerDied", new Godot.Collections.Array() { plyr });
			plyr.Modulate = new Color((100 * (teamCount % 2)) / 255f, (100 * (teamCount % 3)) / 255f, (100 * (teamCount % 4)) / 255f);
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
		DayDeathList = new List<Player>();
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
		if(CurrentState == GameState.EndNight)
		{
			if(DayDeathList.Count > 0)
			{
				foreach (var player in DayDeathList)
				{
					PostMessage($"{player.Name} perished.");
				}
				DayDeathList = new List<Player>();
			}
			else
			{
				PostMessage("No one died.");
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
        {
			GD.Print($"TEAM {player.Team} WINS!!!!!!!");
			PostMessage($"-----TEAM {player.Team} WINS-----");
		}
		else
        {
			GD.Print("EVERYBODY DIED!! BETTER LUCK NEXT TIME!!!!");
			PostMessage($"-----NOBODY WINS-----");
		}

		CurrentState = GameState.GameOver;
	}

	void UpdateScoreboard()
	{
		foreach (Node x in PlayerScoreboard.GetChildren())
		{
			x.QueueFree();
		}

		for (int x = 0; x < PlayerList.Count; x++)
		{
			var label = new Label();
			label.Text = PlayerList[x].PlayerName + " - Team " + PlayerList[x].Team;
			PlayerScoreboard.AddChild(label);
		}
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
				PostMessage("----End of Day----");

				PostProcessActivities();

				if(CurrentState != GameState.GameOver)
                {
					CurrentState = GameState.Night;

					UnprocessedPlayers = new List<Player>(PlayerList);
					PostMessage("----Begin Night----");
				}
				break;

			case GameState.Night:
				ProcessRandomActivity();

				if (UnprocessedPlayers.Count <= 0)
					CurrentState = GameState.EndNight;
				break;

			case GameState.EndNight:
				PostMessage("----End of Night----");

				PostProcessActivities();

				if(CurrentState != GameState.GameOver)
                {
					CurrentState = GameState.Day;

					UnprocessedPlayers = new List<Player>(PlayerList);
					PostMessage("----Begin Day----");
				}
				break;
		}
	}

	void OnPlayerDied(Player player)
	{
		player.CurrentLocale.RemovePlayer(player);
		PlayerList.Remove(player);

		if (UnprocessedPlayers.Contains(player))
			UnprocessedPlayers.Remove(player);

		UpdateScoreboard();

		DayDeathList.Add(player);
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
