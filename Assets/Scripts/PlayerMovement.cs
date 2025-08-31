using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float speed = 0f;

    public float maxSpeed;
    public float accelerationSpeed;

    Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Acceleration();
    }

    private void FixedUpdate()
    {
        Vector3 force = new Vector3(InputManager.Instance.MoveDirection.x, 0, InputManager.Instance.MoveDirection.y);
        Deacceleration(force);
        _rb.MovePosition(_rb.position + speed * Time.fixedDeltaTime * force);
    }

    void Acceleration()
    {
        if (InputManager.Instance.MoveDirection.y != 0f && speed < maxSpeed)
        {
            speed += Time.deltaTime * accelerationSpeed;
        }
        
        //else
        //{
        //    speed -= Time.deltaTime * accelerationSpeed;
        //    if (InputManager.Instance.MoveDirection.y == 0f)
        //    {
        //        speed = 0f;
        //    }
        //}
    }

    void Deacceleration(Vector3 force)
    {
        if(InputManager.Instance.MoveDirection.y == 0f)
        {
            speed -= Time.deltaTime * accelerationSpeed;
            _rb.AddForce(force, ForceMode.Acceleration);
        }

        if (speed <= 0f)
        {
            speed = 0f;
        }
    }
}
