namespace AFSInterview.Combat
{
    using System;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public enum UnitType
    {
        LongSwordKnight = 0,
        Archer = 1,
        Druid = 2,
        Catapult = 3,
        Ram = 4,
    }

    [Flags]
    public enum UnitAttibute
    {
        Light = 1 << 0,
        Armored = 1 << 1,
        Mechanical = 1 << 2,
    }

    public class Unit : MonoBehaviour
    {
        public event Action<int> OnDamageTaken;
        public event Action<Unit> OnDeath;

        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public UnitAttibute Attributes { get; private set; }

        [field: SerializeField, Min(1)] public int MaxHealth { get; private set; }

        [field: SerializeField, Min(0)] public int Armor { get; private set; }

        [field: SerializeField, Min(1), Tooltip("Number of turns that need to pass to perform an attack again, " +
            "value of 1 means unit can attack each turn.")]
        public int AttackInterval { get; private set; }

        [field: SerializeField, Min(0)] public int BaseDamage { get; private set; }

        [SerializeField, SerializedDictionary("Attributes", "Damage")]
        SerializedDictionary<UnitAttibute, int> damageOverrides = new();

        public int CurrentHealth => currentHealth;

        private int currentHealth;
        private int turnsWithoutAttacking;

        private void Awake()
        {
            currentHealth = MaxHealth;
            turnsWithoutAttacking = AttackInterval;
        }

        public void TakeDamage(int damage)
        {
            int actualDamage = Mathf.Max(damage - Armor, 1);
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            OnDamageTaken?.Invoke(actualDamage);
            if (currentHealth == 0)
                OnDeath?.Invoke(this);
        }

        public bool CanAttack()
        {
            turnsWithoutAttacking++;
            if (turnsWithoutAttacking >= AttackInterval)
            {
                turnsWithoutAttacking = 0;
                return true;
            }

            return false;
        }

        public int GetDamageFor(Unit unit)
        {
            if (damageOverrides.Count == 0)
                return BaseDamage;

            // Get override with highest damage match
            int highestDamageOverride = 0;
            foreach (var dmg in damageOverrides)
            {
                int currentMatch = (int)(unit.Attributes & dmg.Key);
                if (currentMatch > 0 && dmg.Value > highestDamageOverride)
                {
                    highestDamageOverride = dmg.Value;
                }
            }

            return highestDamageOverride > 0 ? highestDamageOverride : BaseDamage;
        }
    }
}
