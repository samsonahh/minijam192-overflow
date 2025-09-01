using UnityEngine;

public class Counter : MonoBehaviour
{
    public float knockbackForce = 10f;
    public AudioClip collisionClip;

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
        }
    }
}