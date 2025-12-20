using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;

    [Header("Hareket")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Referanslar")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAttack enemyAttack; // Saldýrý davranýþý ayrý bileþen olarak

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (enemyAttack == null)
            enemyAttack = GetComponent<EnemyAttack>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position);
        float distance = direction.magnitude;
        Vector3 dirNormalized = direction.normalized;

        // Eðer EnemyAttack yoksa sadece takip et
        if (enemyAttack == null)
        {
            transform.position += dirNormalized * moveSpeed * Time.fixedDeltaTime;
            if (animator != null) animator.SetBool("IsMoving", true);
            return;
        }

        // Saldýrý menzilinde mi?
        if (!enemyAttack.IsPlayerInRange(transform, player.transform))
        {
            // Chase
            transform.position += dirNormalized * moveSpeed * Time.fixedDeltaTime;
            if (animator != null) animator.SetBool("IsMoving", true);
        }
        else
        {
            // In attack range: stop and request attack
            if (animator != null) animator.SetBool("IsMoving", false);
            enemyAttack.TryAttack(player.transform);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (enemyAttack != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, /* attackRange : read via enemyAttack */ 0f);
            // Not possible to access private field here; use EnemyAttack gizmos for ranges.
        }
    }
#endif
}
