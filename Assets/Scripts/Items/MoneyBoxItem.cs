namespace AFSInterview.Items
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MoneyBoxItem : Item
    {
        [Header("Consumable")]
        [SerializeField] private int moneyReward;

        public MoneyBoxItem() : base(string.Empty, 0) { }

        public MoneyBoxItem(string name, int value) : base(name, value) { }

        public override bool Use(InventoryController inventoryController)
        {
            inventoryController.AddMoney(moneyReward);
            return true;
        }
    }
}
