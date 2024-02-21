namespace AFSInterview.UI
{
    using AFSInterview.Items;
    using TMPro;
    using UnityEngine;

    public class MoneyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyValue;
        [SerializeField] private InventoryController inventoryController;

        private void Start()
        {
            moneyValue.text = inventoryController.Money.ToString();
            inventoryController.OnMoneyChanged += UpdateMoneyCounter;
        }

        private void OnDestroy()
        {
            inventoryController.OnMoneyChanged -= UpdateMoneyCounter;
        }

        private void UpdateMoneyCounter(int value)
        {
            moneyValue.text = value.ToString();
        }
    }
}
