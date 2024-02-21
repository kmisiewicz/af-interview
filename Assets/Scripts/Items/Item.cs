namespace AFSInterview.Items
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Item
	{
		[SerializeField] private string name = "";
		[SerializeField] private int value;

        public string Name => name;
		public int Value => value;

		public Item() { }

		public Item(string name, int value)
		{
			this.name = name;
			this.value = value;
		}

		public virtual bool Use(InventoryController inventoryController)
		{
			Debug.Log("Can't use basic item - " + Name);
			return false;
		}
	}
}