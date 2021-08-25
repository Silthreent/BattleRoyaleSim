using Godot;

public class Player : Node2D
{
	public string PlayerName { get; protected set; }
	public int Health { get; protected set; }

	public BaseActivity ChooseActivity()
	{
		return new KillActivity();
	}

	public void SetPlayerName(string name)
	{
		PlayerName = name;
	}

	public void LoseHealth()
	{
		Health--;
	}
}
