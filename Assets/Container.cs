using UnityEngine;

public class Container : MonoBehaviour
{
    public AudioClip collisionClip; // Reference to the collision sound

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Play collision sound
            if (collisionClip != null)
                AudioManager.Instance.PlayOneShot(collisionClip);

            // Destroy this container
            Destroy(gameObject);
        }
    }
}