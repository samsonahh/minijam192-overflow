using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;

    Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 force = new Vector3(InputManager.Instance.MoveDirection.x, 0, InputManager.Instance.MoveDirection.y);
        _rb.MovePosition(_rb.position + movementSpeed * Time.fixedDeltaTime * force);
    }
}
