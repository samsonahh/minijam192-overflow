using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float accelerationForce;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 input = new Vector3(InputManager.Instance.MoveDirection.x, 0f, InputManager.Instance.MoveDirection.y);

        if (input != Vector3.zero)
        {
            // Only apply force if max speed hasn't been reached
            if (_rb.linearVelocity.magnitude < maxSpeed)
            {
                _rb.AddForce(input.normalized * accelerationForce, ForceMode.Acceleration);
            }
        }
        else
        {
            ApplyFriction();
        }
    }

    void ApplyFriction()
    {
        // Apply opposing force to slow down naturally
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, Vector3.zero, 0.1f);
    }

    public void SetMaxSpeed(float speed) { maxSpeed = speed; }
    public void SetAccelerationForce(float force) { accelerationForce = force; }
}
