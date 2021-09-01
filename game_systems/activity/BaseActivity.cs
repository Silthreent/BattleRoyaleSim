using System;
using System.Collections.Generic;

public class BaseActivity
{
    public int R_MinEnemyPlayers { get; protected set; } = 0;
    public int R_MaxEnemyPlayers { get; protected set; } = 99;

    public virtual List<Player> Process(Player host, List<Player> interactable)
    {
        return new List<Player>();
    }
}

class ActivityAttribute : Attribute
{

}
