using Godot;
using System.Collections.Generic;

public class Map : Node2D
{
    public static Map CurrentMap { get; protected set; }
    
    List<MapLocale> MapLocales;

    public override void _Ready()
    {
        CurrentMap = this;

        MapLocales = new List<MapLocale>();
        foreach(var x in GetChildren())
        {
            if(x is MapLocale)
                MapLocales.Add((MapLocale)x);
        }

        foreach(var locale in MapLocales)
        {
            foreach(var x in locale.Connections)
            {
                locale.ConnectedLocale.Add(MapLocales[x - 1]);
            }
        }
    }

    public void MovePlayer(Player player, MapLocale locale)
    {
        if (player.CurrentLocale != null)
            player.CurrentLocale.RemovePlayer(player);

        locale.AddPlayer(player);
    }

    public MapLocale GetLocale(int index)
    {
        return MapLocales[index];
    }
}
