using Animancer;
using UnityEngine;
using DG.Tweening; // Make sure DOTween is imported

public class PlayerCombat: MonoBehaviour
{
    public Transform attackPos;
    public float attackRange;

    [SerializeField] private AnimancerComponent animator;
    [SerializeField] private AnimationClip swimClip;
    [SerializeField] private ClipTransition biteClip;
    [SerializeField] private AudioClip sharkBiteClip;

    [Header("Attack Visual Effect")]
    public Transform targetToInflate; // Assign in Inspector
    public float inflateScale = 1.3f;
    public float inflateDuration = 0.15f;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.Attack += SharkAttack;

        animator.Play(swimClip, 0.1f);
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.Attack -= SharkAttack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                timer = 0;
                animator.Play(swimClip, 0.1f);
            }
        }
    }

    void SharkAttack()
    {
        if(timer > 0)
            return;

        animator.Play(biteClip, 0.1f);
        timer = biteClip.Length;
    }

    public void Attack()
    {
        if (sharkBiteClip != null)
            AudioManager.Instance.PlayOneShot(sharkBiteClip);

        Debug.Log("Attack");

        // Inflate effect on the target object every attack
        if (targetToInflate != null)
        {
            targetToInflate.DOKill();
            Vector3 originalScale = targetToInflate.localScale;
            targetToInflate.DOScale(originalScale * inflateScale, inflateDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    targetToInflate.DOScale(originalScale, inflateDuration)
                        .SetEase(Ease.InBack);
                });
        }

        Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange, LayerMask.GetMask("Enemy"));
        if (enemies == null)
            return;

        if (enemies.Length == 0)
            return;

        foreach (Collider enemy in enemies)
        {
            Debug.Log("Hit " + enemy.name);
            if (enemy.TryGetComponent(out Health health))
            {
                health.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    /// <summary>
    /// Instantiates a temporary sphere GameObject with the specified center, radius, expire duration, and color.
    /// The sphere is created as a primitive sphere with a Collider set as a trigger.
    /// The sphere's material is set to be transparent using the Universal Render Pipeline/Unlit shader.
    /// The sphere is destroyed after the specified expire duration.
    /// </summary>
    /// <param name="center">The center position of the sphere.</param>
    /// <param name="radius">The radius of the sphere.</param>
    /// <param name="expireDuration">The duration in seconds before the sphere is destroyed.</param>
    /// <param name="color">The color of the sphere.</param>
    public static GameObject InstantiateTemporarySphere(Vector3 center, float radius, float expireDuration, UnityEngine.Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<Collider>().enabled = false;
        sphere.name = "Temporary Sphere";
        sphere.transform.position = center;
        sphere.transform.localScale = radius * 2f * Vector3.one;

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material = GetTransparentMaterial();
        sphereRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        sphereRenderer.material.color = color;
        GameObject.Destroy(sphere, expireDuration);

        return sphere;
    }

    /// <summary>
    /// Generates a transparent material.
    /// </summary>
    /// <returns>The transparent material.</returns>
    public static Material GetTransparentMaterial()
    {
        Material transparentMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        // Change Surface Type to Transparent
        transparentMaterial.SetFloat("_Surface", 1); // 1 = Transparent, 0 = Opaque

        // Enable required shader keywords
        transparentMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        transparentMaterial.DisableKeyword("_SURFACE_TYPE_OPAQUE");

        // Set rendering mode for transparency
        transparentMaterial.SetOverrideTag("RenderType", "Transparent");
        transparentMaterial.SetInt("_ZWrite", 0); // Disable ZWrite for transparency
        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

        // Apply the changes to the material
        transparentMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        return transparentMaterial;
    }
}
