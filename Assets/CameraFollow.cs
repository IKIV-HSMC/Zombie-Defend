using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Cấu hình di chuyển")]
    [Tooltip("Tốc độ mượt khi camera đuổi theo Player (Càng nhỏ càng mượt)")]
    public float smoothSpeed = 0.125f;
    
    [Tooltip("Khoảng cách bù trục Z (Để tránh camera đè sát vào mặt phẳng 2D, thường là -10)")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void Start()
    {
        // Tự động tìm nhân vật chính bằng Tag giống như cách Zombie tìm Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // Tính toán vị trí đích mà Camera cần tới
        Vector3 desiredPosition = playerTransform.position + offset;
        
        // Nội suy mượt mà từ vị trí hiện tại đến vị trí đích (Lerp)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Cập nhật vị trí mới cho Camera
        transform.position = smoothedPosition;
    }
}