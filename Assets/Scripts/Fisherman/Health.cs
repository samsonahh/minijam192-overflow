using NaughtyAttributes;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: Header("Health Settings")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 10f;
    [field: SerializeField, ReadOnly] public float CurrentHealth { get; private set; }
    [SerializeField] private bool destroyOnDeath = false;

    public event Action<float, float> OnHealthChanged = delegate { }; // before, current
    public event Action OnDeath = delegate { };

    private bool isDead = false;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    /// <summary>
    /// Apply damage. Negative values will heal.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (isDead) 
            return;

        float beforeHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, MaxHealth);
        OnHealthChanged?.Invoke(beforeHealth, CurrentHealth);

        if (CurrentHealth <= 0f)
            Die();
    }

    public void ResetHealth()
    {
        isDead = false;
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        OnDeath?.Invoke();

        if (destroyOnDeath)
            Destroy(gameObject);
    }
}