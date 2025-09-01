using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    private Rigidbody rb;
    private WaterWave waterWave;
    [field: SerializeField, ReadOnly] public bool IsInWater { get; private set; } = false;

    [Header("Config")]
    [SerializeField] private float touchWaterSnapSpeed = 10f;
    [SerializeField] private float bobAmplitude = 2f;
    [SerializeField] private float bobFrequency = 0.75f;
    private float sinParameter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waterWave = Object.FindAnyObjectByType<WaterWave>();
    }

    void FixedUpdate()
    {
        if (waterWave == null) 
            return;

        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        Vector3 pos = transform.position;
        float waterHeight = waterWave.SampleMeshHeight(pos.x, pos.z);
        float bobOffset = EvaluateBobOffset();

        Vector3 targetPosition = new Vector3(pos.x, waterHeight + bobOffset, pos.z);

        if(pos.y < targetPosition.y)
        {
            IsInWater = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, touchWaterSnapSpeed * Time.fixedDeltaTime));
        }
    }

    private float EvaluateBobOffset()
    {
        sinParameter += bobFrequency * Time.fixedDeltaTime;
        return Mathf.Sin(sinParameter) * bobAmplitude;
    }
}