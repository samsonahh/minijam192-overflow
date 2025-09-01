using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SphereCollider))]
public class Microwave : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 3f;
    public Vector3 explosionOffset = Vector3.zero;
    public Color explosionColor = Color.red;
    public float explosionVisualDuration = 0.5f;
    public AudioClip explosionClip; // Reference to the explosion sound
    public Material explosionMaterial; // Assign a material for the explosion sphere

    private Color originalColor;
    private bool exploded = false;
    private SphereCollider col;
    private Material microwaveMaterial;
    private Tween colorTween;

    void Start()
    {
        // Only ensure the collider is a trigger, do not change its size or center
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;

        // Get the material from the Renderer
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            microwaveMaterial = renderer.material; // This gets the instance
            originalColor = microwaveMaterial.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (exploded)
            return;

        if (other.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;
        if (microwaveMaterial != null)
        {
            // Animate color to explosionColor
            colorTween?.Kill();
            colorTween = microwaveMaterial.DOColor(explosionColor, explosionVisualDuration);
        }
        // Play explosion sound
        if (explosionClip != null)
            AudioManager.Instance.PlayOneShot(explosionClip);

        // Visualize explosion as a sphere
        VisualizeExplosionSphere();
    }

    private void VisualizeExplosionSphere()
    {
        // Create a sphere primitive
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position + explosionOffset;
        sphere.transform.localScale = Vector3.zero; // Start small

        // Assign material
        var renderer = sphere.GetComponent<Renderer>();
        if (explosionMaterial != null)
        {
            renderer.material = new Material(explosionMaterial); // Instance
        }
        renderer.material.color = explosionColor;

        
        Destroy(sphere.GetComponent<Collider>());

        // Animate scale
        sphere.transform.DOScale(explosionRadius * 2f, explosionVisualDuration)
            .SetEase(Ease.OutCubic);


        renderer.material.DOFade(0f, explosionVisualDuration)
            .SetDelay(explosionVisualDuration * 0.5f);

        // Destroy the sphere after the animation
        Destroy(sphere, explosionVisualDuration * 1.5f);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exploded = false;
            if (microwaveMaterial != null)
            {
                colorTween?.Kill();
                microwaveMaterial.DOColor(originalColor, explosionVisualDuration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = explosionColor;
        Gizmos.DrawWireSphere(transform.position + explosionOffset, explosionRadius);
    }
}