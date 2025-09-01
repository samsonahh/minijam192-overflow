using UnityEngine;

public class PlayerCombat: MonoBehaviour
{
    public Transform attackPos;
    public float attackRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.Attack += SharkAttack;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.Attack -= SharkAttack;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SharkAttack()
    {
        Debug.Log("Attack");

        Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange);
        if (enemies == null)
            return;

        if (enemies.Length == 0)
            return;

        foreach(Collider enemy in enemies)
        {
            if (enemy.TryGetComponent(out Health health))
            {
                health.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
