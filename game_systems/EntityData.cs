using System;
using System.Collections.Generic;

public class EntityData
{
	public List<BaseEffect> Effects { get; protected set; }
	public List<BaseItem> Inventory { get; protected set; }
	public Player HostPlayer { get; protected set; }
	public MapLocale HostLocale { get; protected set; }

	public EntityData()
	{
		Effects = new List<BaseEffect>();
		Inventory = new List<BaseItem>();
	}

	public void GiveEffect(BaseEffect effect)
	{
		Effects.Add(effect);
	}

	public void LoseEffect(Type eType)
	{
		var effect = Effects.Find(x => x.GetType() == eType);
		if (effect != null)
			Effects.Remove(effect);
	}

	public bool HasEffect(Type effectType)
	{
		return Effects.Find(x => x.GetType() == effectType) != null;
	}

	public void ProcessOnEffects(Action<BaseEffect> process)
    {
		int count = Effects.Count;
		for (int x = 0; x < Effects.Count; x++)
		{
			process.Invoke(Effects[x]);

			if (Effects.Count < count)
				x -= count - Effects.Count;

			count = Effects.Count;
		}
	}

	public T GetEffect<T>() where T : BaseEffect
	{
		return (T)Effects.Find(x => x.GetType() == typeof(T));
	}

	public void GiveItem(BaseItem item)
	{
		Inventory.Add(item);
	}

	public void LoseItem(BaseItem item)
	{
		Inventory.Remove(item);
	}

	public void ProcessOnInventory(Action<BaseItem> process)
	{
		int count = Inventory.Count;
		for (int x = 0; x < Inventory.Count; x++)
		{
			process.Invoke(Inventory[x]);

			if (Inventory.Count < count)
				x -= count - Inventory.Count;

			count = Inventory.Count;
		}
	}

	public void ModifyActivityList(List<BaseActivity> possibles)
    {
		int count = possibles.Count;
		for (int x = 0; x < possibles.Count; x++)
		{
			foreach (var effect in Effects)
			{
				if (!effect.CanDoActivity(possibles[x]))
					possibles.RemoveAt(x);

				if (possibles.Count < count)
					x -= count - possibles.Count;

				count = possibles.Count;
			}
		}
	}

	public void ProcessTimeChange()
	{
		int count = Effects.Count;
		for (int x = 0; x < Effects.Count; x++)
		{
			Effects[x].TickEffect(this);

			if (Effects.Count < count)
				x -= count - Effects.Count;

			count = Effects.Count;
		}
	}
}
