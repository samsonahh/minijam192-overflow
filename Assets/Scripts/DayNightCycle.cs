using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Celestial Objects")]
    public Transform sun;
    public Transform moon;
    public Transform stars;

    [Header("Cycle Settings")]
    public float dayLengthInSeconds = 60f; // Full day-night cycle duration

    [Tooltip("Tilt of the celestial equator (degrees).")]
    public float axisTilt = 23.5f;

    [Header("Material to Animate")]
    public Material targetMaterial; // Assign your regular material here

    private float cycleTimer;

    void Update()
    {
        if (dayLengthInSeconds <= 0f) return;

        // Advance the cycle timer
        cycleTimer += Time.deltaTime;
        float cycle01 = (cycleTimer / dayLengthInSeconds) % 1f; // 0..1

        // Calculate sun and moon angles (opposite each other)
        float sunAngle = cycle01 * 360f;
        float moonAngle = (cycle01 + 0.5f) * 360f;

        Quaternion tilt = Quaternion.Euler(axisTilt, 0f, 0f);

        if (sun != null)
            sun.localRotation = tilt * Quaternion.Euler(sunAngle, 0f, 0f);

        if (moon != null)
            moon.localRotation = tilt * Quaternion.Euler(moonAngle, 0f, 0f);

        if (stars != null)
            stars.localRotation = Quaternion.Euler(0f, sunAngle, 0f);

        // Animate the X offset of the regular material's main texture
        if (targetMaterial != null && targetMaterial.HasProperty("_MainTex"))
        {
            float xOffset = cycle01;
            float yOffset = targetMaterial.GetTextureOffset("_MainTex").y;
            targetMaterial.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
        }
    }
}