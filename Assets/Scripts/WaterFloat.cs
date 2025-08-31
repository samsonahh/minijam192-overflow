using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    public float floatStrength = 10f;
    public float offset = 0f;
    public float normalSampleDistance = 0.5f; // How far to sample for normal estimation
    public float rotationSmooth = 2f; // How quickly the object tilts to match the wave

    private Rigidbody rb;
    public bool isInWater = false;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        // If this object is tagged "Object" and not in water, do nothing (let physics act normally)
        if (CompareTag("Object") && !isInWater)
            return;

        if (!isInWater) return;

        var water = Object.FindAnyObjectByType<WaterWave>();
        if (water == null) return;

        Vector3 pos = rb.position;
        float waterHeight = water.GetWaveHeightAtPosition(pos.x, pos.z);

        // Bobbing: apply upward force if below water
        if (pos.y < waterHeight + offset)
        {
            float force = (waterHeight + offset - pos.y) * floatStrength;
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }

        // Tilting: estimate normal from wave heights at small offsets
        float dx = normalSampleDistance;
        float dz = normalSampleDistance;

        float hCenter = water.GetWaveHeightAtPosition(pos.x, pos.z);
        float hX = water.GetWaveHeightAtPosition(pos.x + dx, pos.z);
        float hZ = water.GetWaveHeightAtPosition(pos.x, pos.z + dz);

        Vector3 tangentX = new Vector3(dx, hX - hCenter, 0f);
        Vector3 tangentZ = new Vector3(0f, hZ - hCenter, dz);
        Vector3 waveNormal = Vector3.Cross(tangentZ, tangentX).normalized;

        // Smoothly tilt the object to match the wave normal
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, waveNormal) * rb.rotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSmooth * Time.fixedDeltaTime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = true;
    }
}