using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [SerializeField] CinemachineCamera freeLookCamera;
    [SerializeField] CinemachineOrbitalFollow orbitalFollow;

    [Range(0f, 1f)]
    public float mouseSensitivity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (freeLookCamera != null)
        {
            orbitalFollow = freeLookCamera.GetComponentInChildren<CinemachineOrbitalFollow>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Camera Look: {InputManager.Instance.LookDirection}");
        Vector2 lookVals = InputManager.Instance.LookDirection;

        if (orbitalFollow != null)
        {

            orbitalFollow.HorizontalAxis.Value += lookVals.x * mouseSensitivity;
            orbitalFollow.VerticalAxis.Value -= lookVals.y * mouseSensitivity * 0.01f; // Y is [0,1]
        }
    }
}
