using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển của nhân vật

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePos;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Tự động tìm Camera chính trong Scene
        cam = Camera.main; 
    }

    void Update()
    {
        // 1. Lấy dữ liệu di chuyển từ bàn phím (W, A, S, D / Mũi tên)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Chuẩn hóa vector để khi đi chéo nhân vật không bị đi nhanh hơn bình thường
        moveInput = moveInput.normalized;

        // 2. Lấy vị trí của con trỏ chuột trên màn hình và đổi sang tọa độ trong Game
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // 3. Di chuyển nhân vật mượt mà bằng Rigidbody2D
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // 4. Xoay nhân vật hướng về phía con trỏ chuột
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        // Trừ đi 90 độ nếu sprite mặc định của bạn hướng lên trên (mặc định Unity góc 0 là bên phải)
        rb.rotation = angle - 90f; 
    }
}