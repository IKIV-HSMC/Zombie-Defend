using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie Data", menuName = "ZombieGame/Zombie Data")]
public class ZombieData : ScriptableObject
{
    public string zombieName;       // Tên loại zombie
    public float maxHealth = 100f;  // Máu tối đa
    public float moveSpeed = 2f;    // Tốc độ di chuyển
    public float damage = 10f;      // Sát thương khi cào pháo đài/player
    public Color zombieColor = Color.red; // Màu sắc để phân biệt ngoại hình
    public Vector3 localScale = Vector3.one; // Kích thước (to hay nhỏ)
    [Header("Visual Assets")]
    public Sprite zombieSprite;
    [Header("Attack Settings")]
    public float attackSpeed = 1f;
}