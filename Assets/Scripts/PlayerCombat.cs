using UnityEngine;

public class PlayerCombat: MonoBehaviour
{
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.Attack += SharkAttack;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SharkAttack()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange, enemyLayer);

        if (enemies.Length > 0)
        {
            Debug.Log(enemies.Length);
            Debug.Log("Enemies Found");
        } else
        {
            Debug.Log(enemies.Length);
            Debug.Log("No Enemies Found");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
