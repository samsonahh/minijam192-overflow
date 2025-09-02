using UnityEngine;

public class Banana : MonoBehaviour
{
    public AudioClip slipClip;

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
                slip.Slip(1.0f); // 1 second slip, or any duration you want
        }
    }
}