using System.Collections.Generic;

public class BaseItem
{
    public string Name { get; protected set; } = "An Item";

    protected BaseActivity[] PossiblesActs;

    public virtual int ModifyAttackRoll(int roll) { return 0; }
    public virtual List<BaseActivity> ModifyActivityList(EntityData owner)
    {
        if (PossiblesActs == null)
            return null;

        var acts = new List<BaseActivity>();
        foreach(var x in PossiblesActs)
        {
            if(x.CanProcess(owner.HostPlayer))
            {
                acts.Add(x);
            }
        }

        return acts;
    }
}
