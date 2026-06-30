using System.Collections.Generic;
using TMPro; // Bắt buộc để dùng TextMeshProUGUI
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("Cấu hình giới hạn")]
    [Tooltip("Số lượng tháp tối đa được phép mang vào trận.")]
    public int maxDeckSize = 4;

    // --- ĐÃ KHAI BÁO LẠI BIẾN NÀY ĐỂ FIX LỖI ẢNH image_064cfd.png ---
    [Header("Tháp Bắt Buộc (Nhà Chính)")]
    [Tooltip("Kéo file dữ liệu Central_Data vào đây")]
    public StructureData centralStructure;

    [Header("Kho Dữ Liệu Tổng")]
    public List<StructureData> allAvailableStructures;

    [Header("Danh Sách Chọn Mang Theo")]
    public List<StructureData> selectedStructures = new List<StructureData>();

    [Header("Giao Diện UI liên kết")]
    public Transform allTowersPanel;
    public Transform selectedTowersPanel;
    public GameObject deckSlotPrefab;

    void Awake()
    {
        // Mỗi lần vào Scene MenuSelection, con DeckManager của Scene đó sẽ làm Instance chính để tránh lỗi mất UI
        Instance = this;
        UpdateDeckUI();
    }

    public void StartGame()
    {
        if (selectedStructures.Count > 0)
        {
            // --- THÊM ĐOẠN NÀY: Bàn giao danh sách tháp đã chọn qua PlayerStatus ---
            if (PlayerStatus.Instance != null)
            {
                PlayerStatus.Instance.savedDeck = new List<StructureData>(selectedStructures);
            }

            // Thay "SampleScene" bằng tên chính xác của Scene trận đấu của bạn
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogWarning("Danh sách tháp đang trống, không thể vào trận!");
        }
        if (selectedStructures.Count > 0)
        {
            // Thay "SampleScene" bằng tên chính xác của Scene trận đấu của bạn
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogWarning("Danh sách tháp đang trống, không thể vào trận!");
        }
    }

    public void UpdateDeckUI()
    {
        // 1. Kiểm tra xem các ô kéo thả UI có bị trống không
        if (allTowersPanel == null)
        {
            Debug.LogError("LỖI CỰC NẶNG: Bạn chưa kéo Panel Kho Tổng vào ô 'All Towers Panel' của _DeckManager!");
            return;
        }
        if (selectedTowersPanel == null)
        {
            Debug.LogError("LỖI CỰC NẶNG: Bạn chưa kéo Panel Tháp Chọn vào ô 'Selected Towers Panel' của _DeckManager!");
            return;
        }
        if (deckSlotPrefab == null)
        {
            Debug.LogError("LỖI CỰC NẶNG: Bạn chưa kéo file Prefab ô tháp vào ô 'Deck Slot Prefab' của _DeckManager!");
            return;
        }

        // Xóa các ô cũ trước khi vẽ lại
        foreach (Transform child in allTowersPanel) Destroy(child.gameObject);
        foreach (Transform child in selectedTowersPanel) Destroy(child.gameObject);

        // 2. Kiểm tra xem kho tháp tổng có dữ liệu không
        Debug.Log("Số lượng tháp có trong Kho Tổng (allAvailableStructures) là: " + allAvailableStructures.Count);

        if (allAvailableStructures.Count == 0)
        {
            Debug.LogWarning("CẢNH BÁO: Danh sách tháp tổng đang bằng 0, nên game không có gì để vẽ lên màn hình!");
        }

        // Sinh các tháp trong KHO TỔNG
        int countTowersCreated = 0;
        foreach (StructureData tower in allAvailableStructures)
        {
            if (centralStructure != null && tower == centralStructure) continue;

            if (!selectedStructures.Contains(tower))
            {
                GameObject slot = Instantiate(deckSlotPrefab, allTowersPanel);
                SetupSlotButton(slot, tower, true);
                countTowersCreated++;
            }
        }
        Debug.Log("Hệ thống đã sinh thành công " + countTowersCreated + " ô tháp lên màn hình Kho Tổng.");

        // Sinh các tháp ĐÃ CHỌN MANG ĐI
        foreach (StructureData tower in selectedStructures)
        {
            GameObject slot = Instantiate(deckSlotPrefab, selectedTowersPanel);
            bool isCentral = (centralStructure != null && tower == centralStructure);
            SetupSlotButton(slot, tower, false, isCentral);
        }
    }

    void SetupSlotButton(GameObject slotObject, StructureData data, bool isChoosing, bool isCentral = false)
    {
        Transform iconTransform = slotObject.transform.Find("Icon");
        if (iconTransform != null)
        {
            Image img = iconTransform.GetComponent<Image>();
            if (img != null && data.structureSprite != null)
            {
                img.sprite = data.structureSprite;
                img.color = Color.white;
            }
        }

        TextMeshProUGUI slotText = slotObject.GetComponentInChildren<TextMeshProUGUI>();
        if (slotText != null)
        {
            if (isCentral)
            {
                slotText.text = "Bắt buộc";
            }
            else
            {
                slotText.text = data.structureName;
            }
        }

        Button btn = slotObject.GetComponent<Button>();
        if (btn != null)
        {
            if (isCentral)
            {
                btn.interactable = false; // Không cho phép click bỏ chọn Nhà Chính
                return;
            }

            if (isChoosing)
            {
                btn.onClick.AddListener(() => SelectTower(data));
            }
            else
            {
                btn.onClick.AddListener(() => DeselectTower(data));
            }
        }
    }

    void SelectTower(StructureData data)
    {
        if (selectedStructures.Count < maxDeckSize)
        {
            selectedStructures.Add(data);
            UpdateDeckUI();
        }
        else
        {
            Debug.LogWarning("Đã đạt giới hạn tối đa " + maxDeckSize + " tháp!");
        }
    }

    void DeselectTower(StructureData data)
    {
        // --- SỬA DÒNG NÀY THÀNH CÓ KIỂM TRA NULL ---
        if (centralStructure != null && data == centralStructure) return;

        if (selectedStructures.Contains(data))
        {
            selectedStructures.Remove(data);
            UpdateDeckUI();
        }
    }


}