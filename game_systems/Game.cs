using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
	public static Game CurrentGame { get; protected set; }

	public static Random RNG { get; protected set; }

	PackedScene PlayerScene;

	List<Player> PlayerList;

	VBoxContainer PlayerScoreboard;
	VBoxContainer MessageLog;
	ScrollContainer MessageScroll;

	GameState CurrentState;
	List<Player> UnprocessedPlayers;

	// Which players died during the current day
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

		// Load every player
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

			counter++;
			if(counter >= 2)
			{
				counter = 0;
				teamCount++;
			}
		}

		UpdateScoreboard();

		UnprocessedPlayers = new List<Player>(PlayerList);
		DayDeathList = new List<Player>();
	}

	/// <summary>
	/// Load a message into the message log.
	/// </summary>
	/// <param name="message">The message to post in the log.</param>
	public void PostMessage(string message)
	{
		var label = new Label();
		label.Text = message;
		label.Autowrap = true;
		MessageLog.AddChild(label);
		MessageLog.MoveChild(label, 0);
	}

	// Pick a random adventurer who hasn't done anything today, and have them do something
	void ProcessRandomActivity()
	{
		GD.Print("--Processing day--");

		var chosenPlayer = UnprocessedPlayers[RNG.Next(0, UnprocessedPlayers.Count)];

		GD.Print($"Doing Activity: {chosenPlayer.PlayerName}");

		var activity = chosenPlayer.ChooseActivity();

		// Any players related to the activity done also can't do anything today
		var involvedPlayers = activity.Process(chosenPlayer);
		foreach (var plyr in involvedPlayers)
		{
			UnprocessedPlayers.Remove(plyr);
		}
		UnprocessedPlayers.Remove(chosenPlayer);
	}

	// End of day/night checks
	void PostProcessActivities()
	{
		// Night ends, post the name of everyone who died
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

		// Check if the game is over, if it is declare a winner
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
		// The important button has been pressed, do something based on game state
		switch(CurrentState)
		{
			// On Day & Night, process an adventurer
			// Then check if any are left to process
			case GameState.Day:
				ProcessRandomActivity();

				if (UnprocessedPlayers.Count <= 0)
					CurrentState = GameState.EndDay;
				break;

			// On End Day & End Night, process end of and then check if game is over
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

			// See comment at day
			case GameState.Night:
				ProcessRandomActivity();

				if (UnprocessedPlayers.Count <= 0)
					CurrentState = GameState.EndNight;
				break;

			// See comment at endday
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

	// A player died, so remove them from their location, the active list, just everywhere
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
