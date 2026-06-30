using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Danh sách tháp mang theo trận này")]
    public StructureData[] loadoutStructures; 

    [Header("Hệ thống liên kết")]
    public BuildManager buildManager;
    public HotbarManager hotbarManager;

    void Start()
    {
        // --- SỬA LẠI LOGIC: Đọc danh sách tháp từ PlayerStatus.Instance ---
        if (PlayerStatus.Instance != null && PlayerStatus.Instance.savedDeck.Count > 0)
        {
            loadoutStructures = PlayerStatus.Instance.savedDeck.ToArray();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy dữ liệu tháp đã chọn, sử dụng cấu hình mặc định trong Inspector!");
        }

        // Giữ nguyên các dòng lệnh nạp tháp vào hotbar của bạn ở phía dưới:
        if (buildManager != null)
        {
            buildManager.availableStructures = loadoutStructures;
        }

        if (hotbarManager != null)
        {
            hotbarManager.SetupHotbar(loadoutStructures);
        }

    }
}