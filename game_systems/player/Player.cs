using Godot;

public class Player : Node2D
{
	public string PlayerName { get; protected set; }

	public void SetPlayerName(string name)
	{
		PlayerName = name;
	}
}
