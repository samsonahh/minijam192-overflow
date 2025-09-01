using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [SerializeField] private WaterWave waterWave;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float rotationSpeed = 15f;
    private float currentSpeed;

    Rigidbody _rb;

    private Vector3 moveDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = Utils.GetCameraBasedMoveInput(Camera.main.transform, InputManager.Instance.MoveDirection);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (waterWave == null)
            return;

        _rb.useGravity = true;
        float height = waterWave.SampleMeshHeight(transform.position.x, transform.position.z);
        if(transform.position.y < height)
            _rb.useGravity = false;

        if(moveDirection != Vector3.zero)
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, maxSpeed);
        else
            currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.fixedDeltaTime, 0, maxSpeed);

        if(moveDirection != Vector3.zero)
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * Time.fixedDeltaTime));
        
        _rb.MovePosition(new Vector3(_rb.position.x, height, _rb.position.z) + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }
}
