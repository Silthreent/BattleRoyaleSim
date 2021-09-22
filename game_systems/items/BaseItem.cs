public class BaseItem
{
    public string Name { get; protected set; }

    public virtual int ModifyAttackRoll(int roll) { return 0; }
}
