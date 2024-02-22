namespace AFSInterview.Visuals
{
    using UnityEngine;

    public class CombatParticlesEmitter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;

        public static CombatParticlesEmitter Instance => instance;

        private static CombatParticlesEmitter instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void EmitAtPosition(Vector3 position, int particleCount = 30)
        {
            transform.position = position;
            particles.Emit(particleCount);
        }
    }
}
