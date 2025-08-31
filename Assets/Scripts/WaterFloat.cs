using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    public float floatStrength = 10f; // How strong the floating force is
    public float offset = 0f; // Offset if you want the object to float above the surface

    private Rigidbody rb;
    private bool isInWater = false;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (!isInWater) return;

        var water = Object.FindAnyObjectByType<WaterWave>();
        if (water == null) return;

        Vector3 pos = transform.position;
        float waterHeight = water.GetWaveHeightAtPosition(pos.x, pos.z);

        if (pos.y < waterHeight + offset)
        {
            float force = (waterHeight + offset - pos.y) * floatStrength;
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water")) // Tag your water collider as "Water"
            isInWater = true;
    }
}