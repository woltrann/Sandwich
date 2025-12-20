using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Saldýrý - Genel")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Kýlýç (Overlap ile hasar)")]
    [SerializeField] private Transform swordPoint;
    [SerializeField] private float swordRange = 0.6f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Animasyon (isteðe baðlý)")]
    [SerializeField] private Animator animator; // Ayný animator'u EnemyMovement ile paylaþabilirsiniz

    private float _lastAttackTime = -Mathf.Infinity;

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
        TriggerAttackAnimation();
    }

    private void TriggerAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }

    // Animasyon event'inden çaðrýlacak: saldýrýnýn tam zamanda hasar vermesini saðlar
    public void DealDamage()
    {
        if (swordPoint == null)
        {
            // Fallback: oyuncu referansýna direkt hasar uygulamak isterseniz dýþarýdan çaðrý yapýlmalý.
            Debug.LogWarning("EnemyAttack: swordPoint atanmadý. Overlap yapýlmadý.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(swordPoint.position, swordRange, playerLayer);
        foreach (Collider hit in hits)
        {
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
                Debug.Log($"EnemyAttack: {hit.name} hasar aldý ({attackDamage}).");
            }
            else
            {
                Debug.LogWarning($"EnemyAttack: Overlap içinde PlayerHealth yok: {hit.name}");
            }
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