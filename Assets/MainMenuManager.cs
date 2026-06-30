using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Các Khung Giao Diện (Panels)")]
    public GameObject mainMenuPanel; // Kéo Khung Menu chính vào đây
    public GameObject settingsPanel; // Kéo Khung Cài đặt vào đây

    [Header("Cấu Hình Component Trong Settings")]
    public Slider volumeSlider;      // Kéo Slider chỉnh âm thanh vào đây
    public Toggle fullScreenToggle;  // Kéo nút Toggle chọn Toàn màn hình vào đây

    void Start()
    {
        // Khi vừa mở game: Hiện Menu chính, ẩn bảng Cài đặt đi
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Tải lại cấu hình âm thanh/đồ họa người chơi đã lưu từ trước (nếu có)
        LoadSettings();
    }

    // 1. CHỨC NĂNG: BẤM NÚT "CHƠI GAME" -> Chuyển sang màn hình chọn tháp
    public void PlayGame()
    {
        // Đổi "MenuSelection" thành tên chính xác của Scene chọn tháp bạn tạo hôm trước
        SceneManager.LoadScene("MenuSelection"); 
    }

    // 2. CHỨC NĂNG: BẬT/TẮT BẢNG SETTING
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        
        // Lưu lại thiết lập khi người chơi đóng bảng cài đặt
        SaveSettings();
    }

    // 3. CHỨC NĂNG: ĐIỀU CHỈNH ĐỒ HỌA & ÂM THANH
    public void SetVolume(float volume)
    {
        // Điều chỉnh âm lượng tổng của toàn hệ thống Unity
        AudioListener.volume = volume;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        // Bật/tắt chế độ toàn màn hình
        Screen.fullScreen = isFullscreen;
    }

    // 4. CHỨC NĂNG: LƯU & TẢI DỮ LIỆU CÀI ĐẶT (Dùng PlayerPrefs của Unity)
    void SaveSettings()
    {
        if (volumeSlider != null)
            PlayerPrefs.SetFloat("GameVolume", volumeSlider.value);
            
        if (fullScreenToggle != null)
            PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
            
        PlayerPrefs.Save();
        Debug.Log("Đã lưu cài đặt hệ thống!");
    }

    void LoadSettings()
    {
        // Load âm lượng (mặc định là tối đa 1.0 nếu chưa từng lưu)
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        if (volumeSlider != null) volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Load chế độ màn hình (mặc định là bật Toàn màn hình = 1)
        int savedFullscreen = PlayerPrefs.GetInt("FullScreen", 1);
        if (fullScreenToggle != null) fullScreenToggle.isOn = (savedFullscreen == 1);
        Screen.fullScreen = (savedFullscreen == 1);
    }

    // 5. CHỨC NĂNG: THOÁT GAME TRÊN MÁY TÍNH
    public void QuitGame()
    {
        Debug.Log("Đang thoát trò chơi...");
        Application.Quit(); // Lệnh này sẽ có tác dụng khi bạn xuất file .exe cài trên máy
    }
}