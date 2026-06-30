using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    [Header("Hệ thống liên kết")]
    public BuildManager buildManager;

    [Header("Cấu hình Prefab & Giao diện")]
    public GameObject slotPrefab;      // Kéo file ô mẫu (Slot Prefab) vào đây
    public Transform hotbarPanel;     // Kéo đối tượng cha (Hotbar) giữ Layout Group vào đây

    private Image[] slotBorders;       // Mảng lưu trữ component Image của Khung Viền (đối tượng Cha)
    private int currentSelectedIndex = 0;

    [Header("Cấu hình màu sắc Viền")]
    public Color selectedColor = Color.yellow; // Màu viền khi được chọn
    public Color normalColor = Color.white;    // Màu viền khi ở trạng thái bình thường

    /// <summary>
    /// Hàm khởi tạo thanh công cụ dựa trên danh sách tháp mang theo trận này.
    /// Được gọi tự động từ LevelManager khi bắt đầu Game.
    /// </summary>
    public void SetupHotbar(StructureData[] structuresToCarry)
    {
        // 1. Dọn dẹp sạch sẽ các ô cũ trong Khung Hotbar nếu có để tránh trùng lặp
        foreach (Transform child in hotbarPanel)
        {
            Destroy(child.gameObject);
        }

        if (structuresToCarry == null || structuresToCarry.Length == 0) return;

        // Khởi tạo kích thước mảng viền bằng đúng số lượng tháp mang theo
        slotBorders = new Image[structuresToCarry.Length];

        // 2. Vòng lặp tự động tạo các ô vuông UI tương ứng với từng loại tháp
        for (int i = 0; i < structuresToCarry.Length; i++)
        {
            int index = i; // Giữ lại chỉ số index cố định để truyền vào sự kiện Click
            StructureData data = structuresToCarry[i];

            // Sinh ra một ô vuông mới từ Prefab mẫu nằm bên trong thanh Hotbar Panel
            GameObject newSlot = Instantiate(slotPrefab, hotbarPanel);
            newSlot.name = "Slot_" + data.structureName;

            // LẤY KHUNG VIỀN: Chính là component Image nằm ngay trên đối tượng gốc (Cha)
            slotBorders[i] = newSlot.GetComponent<Image>();

            // LẤY ICON: Đi tìm đối tượng con tên là "Icon" nằm bên trong để nạp ảnh tháp thu nhỏ
            Transform iconTransform = newSlot.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image iconImage = iconTransform.GetComponent<Image>();
                if (iconImage != null && data.structureSprite != null)
                {
                    // Ép hình ảnh minh họa của Tháp từ file Data vào ô Icon
                    iconImage.sprite = data.structureSprite;

                    // Đảm bảo ảnh tháp hiển thị đúng màu sắc gốc ban đầu
                    iconImage.color = Color.white;
                }
            }
            else
            {
                // Dòng cảnh báo hiển thị ở Console nếu bạn quên chưa tạo đối tượng con tên là Icon
                Debug.LogWarning("Không tìm thấy đối tượng con tên là 'Icon' trong Prefab ô mẫu của bạn!");
            }

            // Cài đặt sự kiện lắng nghe: Khi người chơi dùng chuột Click vào ô này trên UI
            Button btn = newSlot.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => SelectSlot(index));
            }
        }

        // Mặc định tự động chọn ô đầu tiên ngay khi game vừa load xong
        SelectSlot(0);
    }

    void Update()
    {
        if (slotBorders == null || slotBorders.Length == 0) return;

        // Bắt sự kiện bấm phím tắt số trên bàn phím (1, 2, 3, 4...) tương ứng với Minecraft
        for (int i = 0; i < slotBorders.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
                break;
            }
        }
    }

    /// <summary>
    /// Hàm xử lý khi một ô được chọn (bằng phím số hoặc click chuột).
    /// Ra lệnh đổi tháp chuẩn bị xây và xử lý hiệu ứng đổi màu khung viền ngoài.
    /// </summary>
    public void SelectSlot(int index)
    {
        if (buildManager == null || slotBorders == null || index >= slotBorders.Length) return;

        currentSelectedIndex = index;

        // Truyền lệnh sang hệ thống BuildManager để thay đổi loại tháp chuẩn bị xây
        buildManager.SetStructureToBuild(index);

        // VÒNG LẶP ĐỔI MÀU: Chỉ tác động lên mảng Khung Viền, hoàn toàn không chạm vào màu sắc ảnh Tháp bên trong
        for (int i = 0; i < slotBorders.Length; i++)
        {
            if (slotBorders[i] == null) continue;

            // Nếu là ô đang được chọn -> Đổi sang màu Vàng. Các ô còn lại -> Trả về màu Trắng bình thường.
            slotBorders[i].color = (i == index) ? selectedColor : normalColor;
        }
    }
}