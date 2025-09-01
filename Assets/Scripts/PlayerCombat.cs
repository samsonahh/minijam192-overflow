using Animancer;
using UnityEngine;

public class PlayerCombat: MonoBehaviour
{
    public Transform attackPos;
    public float attackRange;

    [SerializeField] private AnimancerComponent animator;
    [SerializeField] private AnimationClip swimClip;
    [SerializeField] private ClipTransition biteClip;
    [SerializeField] private StringAsset biteEvent;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.Attack += SharkAttack;

        biteClip.Events.SetCallback(biteEvent, Attack);
        animator.Play(swimClip, 0.1f);
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
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 0;
                animator.Play(swimClip, 0.1f);
            }
        }
    }

    void SharkAttack()
    {
        animator.Play(biteClip, 0.1f);
        timer = biteClip.Length;
    }

    private void Attack()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange);
        if (enemies == null)
            return;

        if (enemies.Length == 0)
            return;

        foreach (Collider enemy in enemies)
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
