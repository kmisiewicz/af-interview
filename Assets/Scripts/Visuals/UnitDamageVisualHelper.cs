namespace AFSInterview.Visuals
{
    using System.Collections;
    using UnityEngine;
    using AFSInterview.Combat;

    public class UnitDamageVisualHelper : MonoBehaviour
    {
        const string DAMAGE_TINT_PARAM_NAME = "_DamageTintStrength";
        const string DEATH_STRENGTH_PARAM_NAME = "_DeathNoiseStrength";
        private static readonly int DamageTintId = Shader.PropertyToID(DAMAGE_TINT_PARAM_NAME);
        private static readonly int DeathStrengthId = Shader.PropertyToID(DEATH_STRENGTH_PARAM_NAME);

        [SerializeField] private Unit unit;
        [SerializeField] private Renderer unitRenderer;

        [SerializeField] private float damageBlinkDuration = 0.5f;
        [SerializeField] private float deathAnimationDuration = 0.7f;

        private Material material;

        private void Start ()
        {
            material = unitRenderer.material;
            material.SetFloat(DeathStrengthId, 0f);
            unit.OnDamageTaken += VisualizeDamage;
            unit.OnDeath += OnUnitDeath;
        }

        private void OnDestroy()
        {
            unit.OnDamageTaken -= VisualizeDamage;
            unit.OnDeath -= OnUnitDeath;

            material = null;
        }

        private void VisualizeDamage(int damage)
        {
            StopCoroutine(BlinkOnDamage());
            StartCoroutine(BlinkOnDamage());
            CombatParticlesEmitter.Instance.EmitAtPosition(unit.transform.position, 30);
        }

        private IEnumerator BlinkOnDamage()
        {
            float t = 0f;
            while (t < damageBlinkDuration)
            {
                material.SetFloat(DamageTintId, Mathf.Lerp(1f, 0f, t / damageBlinkDuration));
                t += Time.deltaTime;
                yield return null;
            }
            material.SetFloat(DamageTintId, 0f);
        }

        private void OnUnitDeath(Unit unit)
        {
            unit.OnDamageTaken -= VisualizeDamage;
            unit.OnDeath -= OnUnitDeath;

            StartCoroutine(AnimateDeath());
        }

        private IEnumerator AnimateDeath()
        {
            float t = 0f;
            while (t < deathAnimationDuration)
            {
                material.SetFloat(DeathStrengthId, Mathf.Lerp(0f, 1f, t / deathAnimationDuration));
                t += Time.deltaTime;
                yield return null;
            }
            material.SetFloat(DeathStrengthId, 1f);

            unit.gameObject.SetActive(false);
        }
    }
}
