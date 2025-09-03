using UnityEngine;
using DG.Tweening;

public class Banana : MonoBehaviour
{
    public AudioClip slipClip;

    [Header("Squish & Squash Effect")]
    public Transform targetMesh; // Assign this to your mesh/model in the Inspector
    public float squishScaleY = 0.5f;
    public float squashScaleXZ = 1.3f;
    public float squishDuration = 0.12f;

    private Vector3 _originalScale;

    private void Awake()
    {
        if (targetMesh != null)
            _originalScale = targetMesh.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Play slip sound
            if (slipClip != null)
                AudioManager.Instance.PlayOneShot(slipClip);

            // Apply slip effect if the player implements ISlip
            var slip = collision.collider.GetComponent<ISlip>();
            if (slip != null)
                slip.Slip(1.0f);

            // Squish & squash effect on the mesh only
            SquishAndSquash();
        }
    }

    private void SquishAndSquash()
    {
        if (targetMesh == null)
            return;

        targetMesh.DOKill();
        Vector3 squishScale = new Vector3(squashScaleXZ, squishScaleY, squashScaleXZ);

        targetMesh.DOScale(squishScale, squishDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                targetMesh.DOScale(_originalScale, squishDuration)
                    .SetEase(Ease.OutBack);
            });
    }
}