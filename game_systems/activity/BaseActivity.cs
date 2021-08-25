using System.Collections.Generic;

public class BaseActivity
{
    public virtual List<Player> Process(Player host, List<Player> interactable)
    {
        return new List<Player>();
    }
}
