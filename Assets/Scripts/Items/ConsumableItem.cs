namespace AFSInterview.Items
{
    using System;
    using UnityEngine;

    [Serializable]
    public class ConsumableItem : Item
    {
        [Header("Consumable")]
        [SerializeField] private GameObject itemToGiveOnUse;

        public ConsumableItem() : base(string.Empty, 0) { }

        public ConsumableItem(string name, int value) : base(name, value) { }

        public override bool Use(InventoryController inventoryController)
        {
            if (!itemToGiveOnUse.TryGetComponent(out IItemHolder itemHolder))
                return false;

            var item = itemHolder.GetItem(false);
            inventoryController.AddItem(item);
            return true;
        }
    }
}
