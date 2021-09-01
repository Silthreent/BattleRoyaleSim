using System;
using System.Collections.Generic;

public class BaseActivity
{
    // Required number of players to do this activity
    public int R_MinEnemyPlayers { get; protected set; } = 0;
    // The maximum number of players to do this activity
    public int R_MaxEnemyPlayers { get; protected set; } = 99;

    public virtual List<Player> Process(Player host, List<Player> interactable)
    {
        return new List<Player>();
    }
}

class ActivityAttribute : Attribute
{

}
