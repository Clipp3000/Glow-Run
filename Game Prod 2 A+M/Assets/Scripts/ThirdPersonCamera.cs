
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Transform playerBody;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 1.5f;
    public float mouseSensitivity = 100f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerBody != null)
            yaw = playerBody.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target == null || playerBody == null)
            return;

        float mouseX = Input.GetAxis("Mouse X")
            * mouseSensitivity * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y")
            * mouseSensitivity * Time.deltaTime;

        // Mouse rotates the player horizontally
        yaw += mouseX;
        playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Mouse looks up and down
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion cameraRotation =
            Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position;

        Vector3 desiredPosition = targetPosition
            - cameraRotation * Vector3.forward * distance;

        transform.position = desiredPosition;
        transform.rotation = cameraRotation;
    }
}