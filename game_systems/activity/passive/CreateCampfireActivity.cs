using System.Collections.Generic;

[Activity]
public class CreateCampfireActivity : BaseActivity
{
    public override bool CanProcess(Player host)
    {
        if (Game.CurrentGame.CurrentState == GameState.Night)
            return true;

        return false;
    }

    public override List<Player> Process(Player host)
    {
        host.CurrentLocale.Entity.GiveEffect(new CampfireEffect());

        PostMessage($"{host.PlayerName} creates a comfy campfire.");

        return base.Process(host);
    }
}
