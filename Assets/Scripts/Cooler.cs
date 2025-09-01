using UnityEngine;

public class Cooler : MonoBehaviour
{
    [Tooltip("Percentage of max speed to set while slowed (0.5 = 50% speed)")]
    [Range(0f, 1f)]
    public float slowPercent = 0.5f;
    public float slowDuration = 0.2f;
    public float slowRadius = 3f;
    public Vector3 slowOffset = Vector3.zero;
    public AudioClip collisionClip;

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
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + slowOffset, slowRadius);
    }
}