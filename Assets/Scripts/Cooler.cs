using UnityEngine;
using DG.Tweening;

public class Cooler : MonoBehaviour
{
    [Tooltip("Percentage of max speed to set while slowed (0.5 = 50% speed)")]
    [Range(0f, 1f)]
    public float slowPercent = 0.5f;
    public float slowDuration = 0.2f;
    public float slowRadius = 3f;
    public Vector3 slowOffset = Vector3.zero;
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

    void Update()
    {
        // Find all objects tagged "Player" in the scene
        var players = GameObject.FindGameObjectsWithTag("Player");

        Vector3 center = transform.position + slowOffset;

        foreach (var player in players)
        {
            float dist = Vector3.Distance(center, player.transform.position);
            if (dist <= slowRadius)
            {
                var slowable = player.GetComponent<ISlowable>();
                if (slowable != null)
                    slowable.ApplySlowness(slowPercent, slowDuration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collisionClip != null)
                AudioManager.Instance.PlayOneShot(collisionClip);

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + slowOffset, slowRadius);
    }
}