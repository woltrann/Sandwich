using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Gerekli Bileşenler")]
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionAsset inputActions; // Input System dosyan

    [Header("Saldırı Ayarları")]
    [SerializeField] private Transform attackPoint; // Klavyenin ucuna koyacağın boş obje
    [SerializeField] private float attackRange = 0.5f; // Vuruşun etki alanı
    [SerializeField] private LayerMask enemyLayers;

    private InputAction attackAction;

    private void Awake()
    {
        var actionMap = inputActions.FindActionMap("Player");
        attackAction = actionMap.FindAction("Attack");
    }

    private void OnEnable()
    {
        attackAction.Enable();
        attackAction.performed += OnAttackTriggered;
    }

    private void OnDisable()
    {
        attackAction.Disable();
        attackAction.performed -= OnAttackTriggered;
    }

    private void OnAttackTriggered(InputAction.CallbackContext ctx)
    {
        if (animator != null)
        {
            animator.SetTrigger("IsAttack");
        }
    }

    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " kişisine klavye ile vuruldu!");

            // Buraya düşmanın canını azaltma kodu gelecek.
            // Örnek: enemy.GetComponent<EnemyHealth>().TakeDamage(10);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
#endif
}