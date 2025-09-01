using UnityEngine;

public class Cooler : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;
    public AudioClip collisionClip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collisionClip != null)
                AudioManager.Instance.PlayOneShot(collisionClip);

            var slowable = collision.collider.GetComponent<ISlowable>();
            if (slowable != null)
                slowable.ApplySlowness(slowMultiplier, slowDuration);
        }
    }
}