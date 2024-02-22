namespace AFSInterview.Combat
{
    using System.Collections.Generic;
    using System.Linq;
    using AFSInterview.Helpers;
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public class Army : MonoBehaviour
    {
        [SerializeField, SerializedDictionary("Unit Type", "Count")]
        private SerializedDictionary<UnitType, int> startUnits = new();

        [SerializeField] private UnitDefinitions unitDefinitions;

        [SerializeField, Min(1)] private int spawnGridSize = 5;

        [SerializeField, Min(0f)] private float spawnGridCellSize = 1f;

        private List<Unit> units;
        private int unitsProcessedInTurn;

        public bool AllUnitsProcessedThisTurn => unitsProcessedInTurn >= units.Count;
        public bool AnyUnitsAlive => units.Count > 0;

        public void Initialize()
        {
            if (units != null)
            {
                foreach (var unit in units)
                {
                    if (unit != null)
                        Destroy(unit.gameObject);
                }
                units.Clear();
            }
            else
            {
                units = new List<Unit>();
            }

            SpawnUnits();
            units.Shuffle();

            unitsProcessedInTurn = 0;
        }

        public void PrepareForNewTurn()
        {
            unitsProcessedInTurn = 0;
        }

        public Unit GetNextAttacker()
        {
            if (AllUnitsProcessedThisTurn || !AnyUnitsAlive)
                return null;

            while (unitsProcessedInTurn < units.Count)
            {
                if (units[unitsProcessedInTurn].CanAttack())
                    return units[unitsProcessedInTurn++];

                unitsProcessedInTurn++;
            }

            return null;
        }

        /// <summary>
        /// Get Unit with highest damage and lowest health
        /// </summary>
        public Unit GetPriorityUnit()
        {
            if (!AnyUnitsAlive)
                return null;

            return units.GroupBy(unit => unit.BaseDamage)
                .OrderByDescending(g => g.Key)
                .First()
                .Aggregate((unit1, unit2) => unit1.CurrentHealth < unit2.CurrentHealth ? unit1 : unit2);
        }

        public Unit GetRandomUnit()
        {
            if (!AnyUnitsAlive)
                return null;

            return units[Random.Range(0, units.Count)];
        }

        private void SpawnUnits()
        {
            int spawnedUnits = 0;
            Transform parent = transform;
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = parent.rotation;
            foreach (var unit in startUnits)
            {
                if (!unitDefinitions.UnitPrefabs.TryGetValue(unit.Key, out Unit unitPrefab))
                    continue;
                
                for (int i = 0; i < unit.Value; i++)
                {
                    Unit newUnit = Instantiate(unitPrefab, parent);
                    newUnit.transform.SetLocalPositionAndRotation(spawnPosition, spawnRotation);
                    newUnit.OnDeath += OnUnitDied;
                    units.Add(newUnit);

                    spawnedUnits++;
                    if (spawnedUnits % spawnGridSize == 0)
                    {
                        spawnPosition.z = 0f;
                        spawnPosition.x += spawnGridCellSize;
                    }
                    else
                    {
                        spawnPosition.z += spawnGridCellSize;
                    }
                }
            }
        }

        private void OnUnitDied(Unit unit)
        {
            unit.OnDeath -= OnUnitDied;
            units.Remove(unit);
        }
    }
}
