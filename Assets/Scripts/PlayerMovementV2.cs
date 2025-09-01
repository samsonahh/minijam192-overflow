using System;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [SerializeField] private WaterWave waterWave;
    [SerializeField] private GameObject boat;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float stunSpeed = 50f;
    private float currentSpeed;

    Rigidbody _rb;

    private Vector3 moveDirection;

    private float stunTimer;
    private Vector3 pushDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        waterWave.OnPulse += WaterWave_OnPulse;
    }

    private void OnDestroy()
    {
        if (waterWave != null)
            waterWave.OnPulse -= WaterWave_OnPulse;
    }

    private void Update()
    {
        moveDirection = Utils.GetCameraBasedMoveInput(Camera.main.transform, InputManager.Instance.MoveDirection);

        if(stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if(stunTimer < 0)
                stunTimer = 0;

            currentSpeed = 0f;
        }
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

        if(stunTimer == 0)
        {
            if (moveDirection != Vector3.zero)
                currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, maxSpeed);
            else
                currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.fixedDeltaTime, 0, maxSpeed);

            if (moveDirection != Vector3.zero)
                _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * Time.fixedDeltaTime));

            _rb.MovePosition(new Vector3(_rb.position.x, height, _rb.position.z) + transform.forward * currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0f, stunSpeed * stunTimer / 2f, 0f), Space.World);
            _rb.MovePosition(new Vector3(_rb.position.x, height, _rb.position.z) + pushDirection * stunSpeed * stunTimer * Time.fixedDeltaTime);
        }
    }

    private void WaterWave_OnPulse()
    {
        stunTimer = stunDuration;

        pushDirection = transform.position - boat.transform.position;
        pushDirection.y = 0f;
        pushDirection.Normalize();
    }
}
