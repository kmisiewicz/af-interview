namespace AFSInterview.Items
{
    using System;
    using System.Collections.Generic;
	using UnityEngine;

	public class InventoryController : MonoBehaviour
	{
		public event Action<int> OnMoneyChanged;

		[SerializeField] private List<Item> items;
		[SerializeField] private int money;

		public int Money => money;
		public int ItemsCount => items.Count;

        private void Awake()
        {
            items ??= new List<Item>();
        }

        public void SellAllItemsUpToValue(int maxValue)
		{
			int startItemsCount = items.Count;
            for (var i = startItemsCount - 1; i >= 0; i--)
			{
				var itemValue = items[i].Value;
				if (itemValue > maxValue)
					continue;
				
				money += itemValue;
				items.RemoveAt(i);
			}

			if (items.Count < startItemsCount)
				OnMoneyChanged?.Invoke(money);

			Debug.Log($"Sold {startItemsCount - items.Count} items worth up to {maxValue} each, remaining items: {items.Count}");
		}

		public void AddItem(Item item)
		{
			items.Add(item);
		}
	}
}