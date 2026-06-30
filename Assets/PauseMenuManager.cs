using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // THƯ VIỆN BẮT BUỘC ĐỂ ĐIỀU KHIỂN TEXTMESHPRO

public class PauseMenuManager : MonoBehaviour
{
    [Header("Giao diện Menu UI")]
    public GameObject pauseMenuPanel;

    [Header("Bảng Thống Kê Status (TextMeshPro)")]
    // Đã chuyển toàn bộ sang kiểu dữ liệu TextMeshProUGUI chuẩn UI mới
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI zombieKilledText;
    public TextMeshProUGUI wavesClearedText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI damageBuffText;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // KHI MENU ĐANG MỞ: Liên tục ép cập nhật số liệu theo thời gian thực mỗi khung hình
        if (isPaused)
        {
            UpdateStatusBoard();
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        UpdateStatusBoard(); // Cập nhật ngay lập tức lúc vừa mở tab
    }

    void UpdateStatusBoard()
    {
        if (PlayerStatus.Instance == null) return;

        // Tiến hành ghi đè dữ liệu từ hệ thống PlayerStatus lên các dòng chữ UI tương ứng
        if (levelText != null)
            levelText.text = "CẤP ĐỘ: " + PlayerStatus.Instance.currentLevel;

        if (zombieKilledText != null)
            zombieKilledText.text = "ZOMBIE ĐÃ DIỆT: " + PlayerStatus.Instance.zombieKilled;

        if (wavesClearedText != null)
            wavesClearedText.text = "WAVE ĐÃ QUA: " + PlayerStatus.Instance.wavesCleared;

        if (goldText != null)
            goldText.text = "VÀNG HIỆN CÓ: " + PlayerStatus.Instance.currentGold;

        if (damageBuffText != null)
        {
            float buffPercent = PlayerStatus.Instance.damageBuffPercent * 100f;
            damageBuffText.text = "BUFF SÁT THƯƠNG: +" + buffPercent + "%";
        }
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting");
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Trả lại thời gian thực

        // Reset toàn bộ chỉ số + kho tháp về mặc định
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.ResetStatus();
        }

        // Chuyển về màn hình Menu chính (Hãy đổi thành "MainMenu" nếu tên Scene của bạn là vậy)
        SceneManager.LoadScene("MainMenu");
    }
}