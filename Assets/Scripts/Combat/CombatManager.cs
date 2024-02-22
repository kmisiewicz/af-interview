namespace AFSInterview.Combat
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class CombatManager : MonoBehaviour
    {
        public event Action<Unit, Unit> OnAttack;
        public event Action<int> OnTurnChange;
        public event Action<Army> OnCombatComplete;

        [SerializeField, Tooltip("Holds references to exactly 2 armies that will fight each other.")]
        private Army[] armies = new Army[2];

        private bool allArmiesFinished;
        private int turn;

        public void StartCombat()
        {
            StartCoroutine(CombatCoroutine());
        }

        private IEnumerator CombatCoroutine()
        {
            InitializeArmies();

            yield return null;

            turn = 0;
            // Loop for turns
            while (true)
            {
                turn++;
                OnTurnChange?.Invoke(turn);

                foreach (Army army in armies)
                    army.PrepareForNewTurn();

                yield return new WaitForSeconds(0.5f);

                allArmiesFinished = true;
                Army army1 = armies[0];
                Army army2 = armies[1];

                // Loop for each move (one unit from each army) until all perform their attacks
                while (true)
                {
                    yield return PerformAttack(army1, army2);
                    yield return PerformAttack(army2, army1);

                    if (allArmiesFinished)
                        break;
                }

                if (!army1.AnyUnitsAlive)
                {
                    yield return FinishCombat(army2);
                    break;
                }
                else if (!army2.AnyUnitsAlive)
                {
                    yield return FinishCombat(army1);
                    break;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator PerformAttack(Army attackingArmy, Army armyToAttack)
        {
            if (attackingArmy.AllUnitsProcessedThisTurn || !armyToAttack.AnyUnitsAlive)
            {
                allArmiesFinished = true;
                yield break;
            }

            allArmiesFinished = false;

            Unit attacker = attackingArmy.GetNextAttacker();
            if (attacker == null)
                yield break;

            Unit unitToAttack = armyToAttack.GetPriorityUnit();
            if (unitToAttack == null)
                yield break;

            OnAttack?.Invoke(attacker, unitToAttack);
            yield return new WaitForSeconds(1f);

            int damage = attacker.GetDamageFor(unitToAttack);
            Debug.Log($"{attacker.Name} --({damage})-> {unitToAttack.Name}");
            unitToAttack.TakeDamage(damage);
        }

        private IEnumerator FinishCombat(Army winnerArmy)
        {
            OnCombatComplete?.Invoke(winnerArmy);
            yield return null;
        }

        private void InitializeArmies()
        {
            if (UnityEngine.Random.value > 0.5f)
                (armies[0], armies[1]) = (armies[1], armies[0]);

            foreach (Army army in armies)
                army.Initialize();
        }

        private void OnValidate()
        {
            if (armies.Length < 2)
            {
                Army[] newArmies = new Army[2];
                for (int i = 0; i < armies.Length; i++)
                    newArmies[i] = armies[i];
                armies = newArmies;
            }
            else if (armies.Length > 2)
            {
                Army[] newArmies = new Army[2];
                for (int i = 0; i < 2; i++)
                    newArmies[i] = armies[i];
                armies = newArmies;
            }
        }
    }
}
