using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    [Header("References")]
    public Transform head; // Player içindeki Head objesi

    [Header("Settings")]
    public float sensitivity = 2.0f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    float yaw;
    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Mevcut rotasyonu baz al
        yaw = transform.eulerAngles.y;
        pitch = head.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X") * sensitivity;
        float my = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mx;
        pitch -= my;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Yaw: Player gövdesi döner
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Pitch: sadece kafa yukarý-aþaðý bakar
        head.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
