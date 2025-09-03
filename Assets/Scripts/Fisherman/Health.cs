using NaughtyAttributes;
using System;
using UnityEngine;
using DG.Tweening;

public class Health : MonoBehaviour
{
    [field: Header("Health Settings")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 10f;
    [field: SerializeField, ReadOnly] public float CurrentHealth { get; private set; }
    [SerializeField] private bool destroyOnDeath = false;

    [Header("Visual Feedback")]
    public GameObject targetObject; // Assign the mesh GameObject in the Inspector
    public float squishScaleY = 0.5f;
    public float squashScaleXZ = 1.3f;
    public float squishDuration = 0.12f;
    public float blinkDuration = 0.12f;
    public Color blinkColor = Color.red;

    public event Action<float, float> OnHealthChanged = delegate { }; // before, current
    public event Action OnDeath = delegate { };

    private bool isDead = false;
    private Vector3 _originalScale;
    private Color _originalColor;
    private Material _matInstance;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (targetObject != null)
        {
            _originalScale = targetObject.transform.localScale;

            // Get and instance the material for color changes
            var renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                _matInstance = renderer.material = new Material(renderer.material);
                _originalColor = _matInstance.color;
            }
        }
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

        // Visual feedback
        if (targetObject != null)
        {
            SquishAndSquash();
            BlinkRed();
        }

        if (CurrentHealth <= 0f)
            Die();
    }

    private void SquishAndSquash()
    {
        var t = targetObject.transform;
        t.DOKill();
        Vector3 squishScale = new Vector3(squashScaleXZ, squishScaleY, squashScaleXZ);

        t.DOScale(squishScale, squishDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                t.DOScale(_originalScale, squishDuration)
                    .SetEase(Ease.OutBack);
            });
    }

    private void BlinkRed()
    {
        if (_matInstance == null)
            return;

        _matInstance.DOKill();
        _matInstance.DOColor(blinkColor, blinkDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _matInstance.DOColor(_originalColor, blinkDuration)
                    .SetEase(Ease.OutQuad);
            });
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