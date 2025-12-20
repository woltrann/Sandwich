using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    [Header("Saldırı - Genel")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDuration = 0.8f; // ✅ EKLENDİ

    [Header("Kılıç (Overlap ile hasar)")]
    [SerializeField] private Transform swordPoint;
    [SerializeField] private float swordRange = 0.6f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Animasyon")]
    [SerializeField] private Animator animator;

    private float _lastAttackTime = -Mathf.Infinity;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public bool IsPlayerInRange(Transform enemyTransform, Transform playerTransform)
    {
        if (playerTransform == null || enemyTransform == null) return false;

        float sqrDistance = (playerTransform.position - enemyTransform.position).sqrMagnitude;
        return sqrDistance <= attackRange * attackRange;
    }

    public void TryAttack(Transform playerTransform)
    {
        if (playerTransform == null) return;
        if (Time.time - _lastAttackTime < attackCooldown) return;
        if (!IsPlayerInRange(transform, playerTransform)) return;

        _lastAttackTime = Time.time;

        // === ATTACK STATE BAŞLAT ===
        enemyMovement?.StartAttack();
        Invoke(nameof(EndAttack), attackDuration);

        TriggerAttackAnimation();
    }

    private void EndAttack()
    {
        enemyMovement?.EndAttack();
    }

    private void TriggerAttackAnimation()
    {
        animator?.SetTrigger("Attack");
    }

    // Animasyon event
    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(swordPoint.position, swordRange, playerLayer);
        foreach (Collider hit in hits)
        {
            hit.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (swordPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(swordPoint.position, swordRange);
        }
    }
#endif
}
