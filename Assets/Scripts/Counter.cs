using UnityEngine;
using DG.Tweening;

public class Counter : MonoBehaviour
{
    public float knockbackForce = 10f;
    public AudioClip collisionClip;

    [Header("Squish & Squash Effect")]
    public GameObject targetObject; // Assign in Inspector
    public float squishScaleY = 0.5f;
    public float squashScaleXZ = 1.3f;
    public float squishDuration = 0.12f;

    private Vector3 _originalScale;

    private void Awake()
    {
        if (targetObject != null)
            _originalScale = targetObject.transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collisionClip != null)
                AudioManager.Instance.PlayOneShot(collisionClip);

            var knockbackable = collision.collider.GetComponent<IKnockbackable>();
            if (knockbackable != null)
            {
                Vector3 direction = (collision.collider.transform.position - transform.position).normalized;
                knockbackable.ApplyKnockback(direction, knockbackForce);
            }

            SquishAndSquash();
        }
    }

    private void SquishAndSquash()
    {
        if (targetObject == null)
            return;

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
}