using Animancer;
using System;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour, ISlowable, IKnockbackable, ISlip
{
    [SerializeField] private WaterWave waterWave;
    [SerializeField] private GameObject boat;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float stunSpeed = 50f;
    private float currentSpeed;

    private Rigidbody _rb;

    private Vector2 inputMoveDirection;
    private Vector3 moveDirection;

    private float stunTimer;
    private Vector3 pushDirection;

    // Slowness and Knockback variables
    private float slownessTimer = 0f;
    private float slownessPercent = 1f;
    private float knockbackTimer = 0f;
    private Vector3 knockbackDirection;
    private float knockbackForce;

    private float originalMaxSpeed;

    // Slip state
    private bool isSlipped = false;
    private float slipTimer = 0f;

    // Visualizer fields
    [Header("Slowness Visualizer")]
    [SerializeField] private Renderer targetRenderer; // Assign your player mesh renderer here
    [SerializeField] private float slownessFadeDuration = 0.2f;
    private Material _slownessMatInstance;
    private Color _originalColor;
    private bool _isSlownessVisualActive = false;
    private Coroutine _slownessCoroutine;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        waterWave.OnPulse += WaterWave_OnPulse;
        originalMaxSpeed = maxSpeed;

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
        {
            _slownessMatInstance = targetRenderer.material = new Material(targetRenderer.material);
            _originalColor = _slownessMatInstance.color;
        }
    }

    private void OnDestroy()
    {
        if (waterWave != null)
            waterWave.OnPulse -= WaterWave_OnPulse;
    }

    private void Update()
    {
        if (InputManager.Instance == null)
            return;

        inputMoveDirection = InputManager.Instance.MoveDirection;
        moveDirection = Utils.GetCameraBasedMoveInput(Camera.main.transform, inputMoveDirection);

        // Handle slip timer
        if (slipTimer > 0)
        {
            slipTimer -= Time.deltaTime;
            if (slipTimer <= 0)
            {
                slipTimer = 0f;
                if (isSlipped)
                {
                    var rot = transform.rotation.eulerAngles;
                    rot.z = 0f;
                    transform.rotation = Quaternion.Euler(rot);
                    isSlipped = false;
                    _rb.linearVelocity = Vector3.zero;
                }
            }
        }

        if (isSlipped)
        {
            var rot = transform.rotation.eulerAngles;
            if (Mathf.Abs(Mathf.DeltaAngle(rot.z, 180f)) > 0.01f)
            {
                rot.z = 180f;
                transform.rotation = Quaternion.Euler(rot);
            }
            currentSpeed = 0f;
        }

        // Handle stun timer
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0)
            {
                stunTimer = 0;
                _rb.linearVelocity = Vector3.zero;
            }
            if (!isSlipped)
                currentSpeed = 0f;
        }

        // slowness timer
        if (slownessTimer > 0)
        {
            slownessTimer -= Time.deltaTime;
            if (slownessTimer <= 0)
            {
                slownessPercent = 1f;
                slownessTimer = 0f;
                if (_isSlownessVisualActive)
                {
                    if (_slownessCoroutine != null)
                        StopCoroutine(_slownessCoroutine);
                    _slownessCoroutine = StartCoroutine(LerpSlownessColor(_slownessMatInstance.color, _originalColor, slownessFadeDuration));
                    _isSlownessVisualActive = false;
                }
            }
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
        if (transform.position.y < height)
            _rb.useGravity = false;

        float effectiveMaxSpeed = maxSpeed * slownessPercent;

        if (stunTimer == 0)
        {
            if (moveDirection != Vector3.zero)
                currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, effectiveMaxSpeed);
            else
                currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.fixedDeltaTime, 0, effectiveMaxSpeed);

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

    // ISlowable implementation
    public void ApplySlowness(float maxSpeedPercent, float duration)
    {
        slownessPercent = Mathf.Clamp01(maxSpeedPercent);
        slownessTimer = duration;

        // Start visual effect
        if (targetRenderer != null && _slownessMatInstance != null)
        {
            Color transparentBlue = new Color(0.2f, 0.5f, 1f, 0.5f); // semi-transparent blue
            if (_slownessCoroutine != null)
                StopCoroutine(_slownessCoroutine);
            _slownessCoroutine = StartCoroutine(LerpSlownessColor(_slownessMatInstance.color, transparentBlue, slownessFadeDuration));
            _isSlownessVisualActive = true;
        }
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        stunTimer = 0.2f;
        pushDirection = direction.normalized;

        // Apply knockback force directly to the Rigidbody
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero; // Reset current velocity for consistent knockback
            _rb.AddForce(direction.normalized * force, ForceMode.VelocityChange);
        }
    }

    // ISlip implementation
    public void Slip(float duration)
    {
        var rot = transform.rotation.eulerAngles;
        rot.z = 180f;
        transform.rotation = Quaternion.Euler(rot);

        isSlipped = true;
        slipTimer = duration;
    }

    // Coroutine to lerp color/alpha
    private System.Collections.IEnumerator LerpSlownessColor(Color from, Color to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            if (_slownessMatInstance != null)
                _slownessMatInstance.color = Color.Lerp(from, to, t / duration);
            yield return null;
        }
        if (_slownessMatInstance != null)
            _slownessMatInstance.color = to;
    }
}