namespace AFSInterview.UI
{
    using AFSInterview.Combat;
    using TMPro;
    using UnityEngine;

    public class CombatDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private TextMeshProUGUI attackerName;
        [SerializeField] private TextMeshProUGUI targetName;
        [SerializeField] private TextMeshProUGUI turnLabel;
        [SerializeField] private TextMeshProUGUI finishMessage;
        [SerializeField] private GameObject namesContainer;

        private void Start()
        {
            combatManager.OnAttack += ShowAttackUI;
            combatManager.OnTurnChange += OnTurnChange;
            combatManager.OnCombatComplete += OnCombatComplete;

            finishMessage.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (combatManager != null)
            {
                combatManager.OnAttack -= ShowAttackUI;
                combatManager.OnTurnChange -= OnTurnChange;
                combatManager.OnCombatComplete -= OnCombatComplete;
            }
        }

        public void Show()
        {
            container.SetActive(true);
            finishMessage.gameObject.SetActive(false);
            namesContainer.SetActive(true);
        }

        private void ShowAttackUI(Unit attacker, Unit target)
        {
            attackerName.text = attacker.Name;
            targetName.text = target.Name;
        }

        private void OnTurnChange(int turn)
        {
            turnLabel.text = turn.ToString();
        }

        private void OnCombatComplete(Army winArmy)
        {
            namesContainer.SetActive(false);
            finishMessage.gameObject.SetActive(true);
            finishMessage.text = $"{winArmy.name} wins";
        }
    }
}
