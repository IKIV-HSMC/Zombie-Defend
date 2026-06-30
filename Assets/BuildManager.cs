using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public StructureData[] availableStructures; // Danh sách công trình có thể xây
    private StructureData structureToBuild;

    void Update()
    {
        // Bấm phím 1, 2, 3, 4 để chọn loại công trình muốn xây
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetStructureToBuild(0); // Trung tâm
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetStructureToBuild(1); // Phòng thủ (Tường)
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetStructureToBuild(2); // Tấn công (Trụ)
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetStructureToBuild(3); // Sản xuất (Nhà vàng)

        // Click chuột trái để đặt công trình xuống map
        if (Input.GetMouseButtonDown(1) && structureToBuild != null) // Dùng chuột phải để đặt để tránh trùng nút bắn súng
        {
            BuildStructureAtMouse();
        }
    }

    public void SetStructureToBuild(int index)
    {
        if (index < availableStructures.Length)
        {
            structureToBuild = availableStructures[index];
            Debug.Log("Đang chọn xây: " + structureToBuild.structureName);
        }
    }

    void BuildStructureAtMouse()
    {
        // Lấy tọa độ chuột chuyển sang không gian World
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Cơ chế GRID SNAP: Làm tròn tọa độ về số nguyên để các công trình xếp thẳng hàng vuông vức
        float snappedX = Mathf.Round(mousePos.x);
        float snappedY = Mathf.Round(mousePos.y);
        Vector3 spawnPos = new Vector3(snappedX, snappedY, 0f);

        // Kiểm tra xem vị trí này đã có công trình nào chưa (tránh xây đè)
        Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.4f);
        if (hit == null)
        {
            GameObject newBuilding = Instantiate(structureToBuild.structurePrefab, spawnPos, Quaternion.identity);
            newBuilding.GetComponent<Structure>().data = structureToBuild;
            Debug.Log("Đã xây xong " + structureToBuild.structureName);
        }
        else
        {
            Debug.LogWarning("Vị trí này đã bị chặn, không thể xây!");
        }
    }
}