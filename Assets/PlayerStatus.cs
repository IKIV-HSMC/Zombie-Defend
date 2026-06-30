using UnityEngine;
using System.Collections.Generic; // Đảm bảo có dòng này ở đầu file


public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;

    // --- THÊM DÒNG NÀY ĐỂ LƯU DANH SÁCH THÁP CHỌN XUYÊN SCENE ---
    [Header("Bộ tháp người chơi mang theo")]
    public List<StructureData> savedDeck = new List<StructureData>();

    [Header("--- CẤP ĐỘ & THÀNH TÍCH ---")]
    public int currentLevel = 1;
    public int zombieKilled = 0;
    public int wavesCleared = 0;

    [Header("--- TÀI CHÍNH ---")]
    public int currentGold = 0;

    [Header("--- HỆ THỐNG BUFF (%) ---")]
    [Tooltip("Tỷ lệ tăng thêm sát thương (Ví dụ: 0.1 là tăng 10% damage)")]
    public float damageBuffPercent = 0f; 
    
    [Tooltip("Tỷ lệ tăng tốc độ đánh (Ví dụ: 0.15 là tăng 15% tốc đánh)")]
    public float attackSpeedBuffPercent = 0f;

    [Tooltip("Tỷ lệ tăng máu tối đa cho công trình hoặc nhân vật")]
    public float maxHealthBuffPercent = 0f;

    void Awake()
    {
        // Cơ chế Singleton để giữ các chỉ số này không bị mất khi chuyển Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Hàm gọi mỗi khi một Zombie bị tiêu diệt
    /// </summary>
    public void AddZombieKill(int goldReward)
    {
        zombieKilled++;
        currentGold += goldReward;

        // Logic tăng cấp: Cứ mỗi 20 con Zombie diệt được thì tăng 1 Cấp (Bạn có thể sửa lại số này)
        int NewLevel = (zombieKilled / 20) + 1;
        if (NewLevel > currentLevel)
        {
            currentLevel = NewLevel;
            Debug.Log("CHÚC MỪNG! Bạn đã lên cấp: " + currentLevel);
            
            // Tự động thưởng thêm buff mỗi khi lên cấp (Ví dụ: mỗi cấp tăng thêm 5% damage)
            damageBuffPercent += 0.05f;
        }
    }

    /// <summary>
    /// Hàm gọi khi vượt qua một Wave (Đợt quái)
    /// </summary>
    public void AddWaveCleared()
    {
        wavesCleared++;
    }

    /// <summary>
    /// Tiện ích lấy chỉ số sát thương sau khi đã cộng thêm % Buff
    /// </summary>
    public float GetBuffedDamage(float baseDamage)
    {
        return baseDamage * (1f + damageBuffPercent);
    }
    public void ResetStatus()
    {
        currentLevel = 1;
        zombieKilled = 0;
        wavesCleared = 0;
        currentGold = 0;
        damageBuffPercent = 0f;
        attackSpeedBuffPercent = 0f;
        maxHealthBuffPercent = 0f;

        // --- SỬA LẠI ĐOẠN NÀY: Xóa sạch bộ tháp lưu trữ khi out game ---
        savedDeck.Clear();

        Debug.Log("Đã reset toàn bộ trạng thái và kho tháp về mặc định!");
    }
}