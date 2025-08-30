using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    public WaterWave water;
    public float floatStrength = 10f; // How strong the floating force is
    public float offset = 0f; // Offset if you want the object to float above the surface

    private Rigidbody rb;

    void Awake()
    {
        // Try to find WaterWave on the same GameObject first
        water = GetComponent<WaterWave>();
        // If not found, search the scene for any WaterWave
        if (water == null)
            water = Object.FindAnyObjectByType<WaterWave>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (water == null) return;

        Vector3 pos = transform.position;
        float waterHeight = water.GetWaveHeightAtPosition(pos.x, pos.z);

        if (pos.y < waterHeight + offset)
        {
            float force = (waterHeight + offset - pos.y) * floatStrength;
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }
}