using UnityEngine;

public enum StructureType { CENTRAL, ATTACK, DEFENSE, PRODUCTION }

[CreateAssetMenu(fileName = "New Structure Data", menuName = "ZombieGame/Structure Data")]
public class StructureData : ScriptableObject
{
    public string structureName;
    public StructureType type;
    public float maxHealth = 200f;
    public int cost = 50;              // Số tiền cần để xây
    public Sprite structureSprite;     // Hình ảnh công trình
    public GameObject structurePrefab; // Prefab thực thể khi xây xong
}