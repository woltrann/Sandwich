using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;

    [Header("Hareket")]
    [SerializeField] private float moveSpeed = 3f;
    private bool isAttacking = false;

    [Header("Rotasyon")]
    [SerializeField] private float rotationSpeed = 8f;

    [Header("Referanslar")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAttack enemyAttack;
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (enemyAttack == null)
            enemyAttack = GetComponent<EnemyAttack>();

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        if (isAttacking) return;

        Vector3 direction = (player.transform.position - transform.position);
        direction.y = 0f;
        Vector3 dirNormalized = direction.normalized;

        if (enemyAttack != null && enemyAttack.IsPlayerInRange(transform, player.transform))
        {
            LookAtPlayer(dirNormalized);
            animator?.SetBool("IsMoving", false);
            enemyAttack.TryAttack(player.transform);
            return;
        }

        // Chase
        LookAtPlayer(dirNormalized);
        transform.position += dirNormalized * moveSpeed * Time.fixedDeltaTime;
        animator?.SetBool("IsMoving", true);
    }
    private void LookAtPlayer(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }
    public void StartAttack()
    {
        isAttacking = true;
        animator?.SetBool("IsMoving", false);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        //if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

       
    }
}
