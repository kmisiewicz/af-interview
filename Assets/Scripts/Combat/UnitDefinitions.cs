namespace AFSInterview.Combat
{
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitDefinitions", menuName = "AFInterview/ScriptableObjects/UnitDefinitions")]
    public class UnitDefinitions : ScriptableObject
    {
        [SerializedDictionary("Unit Type", "Prefab")]
        public SerializedDictionary<UnitType, Unit> UnitPrefabs = new();
    }
}
