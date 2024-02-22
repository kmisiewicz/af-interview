namespace AFSInterview.Visuals
{
    using System.Collections;
    using AFSInterview.Combat;
    using UnityEngine;

    public class UnitReticleController : MonoBehaviour
    {
        [SerializeField] private CombatManager combatManager;
        [SerializeField] private GameObject attackerReticle;
        [SerializeField] private GameObject targetReticle;
        [SerializeField] private float highlightDuration = 0.7f;

        private void Start()
        {
            SetReticlesActive(false);

            combatManager.OnAttack += OnAttack;
        }

        private void OnDestroy()
        {
            combatManager.OnAttack -= OnAttack;            
        }

        private void OnAttack(Unit attacker, Unit target)
        {
            Vector3 position = attacker.transform.position;
            attackerReticle.transform.position = position;

            position = target.transform.position;
            targetReticle.transform.position = position;

            StopCoroutine(HighlightCoroutine());
            StartCoroutine(HighlightCoroutine());
        }

        private IEnumerator HighlightCoroutine()
        {
            SetReticlesActive(true);
            yield return new WaitForSeconds(highlightDuration);
            SetReticlesActive(false);
        }

        private void SetReticlesActive(bool active)
        {
            attackerReticle.SetActive(active);
            targetReticle.SetActive(active);
        }
    }
}
