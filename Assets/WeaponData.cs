using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ZombieGame/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;       // Tên súng
    public float fireRate = 0.2f;    // Thời gian giãn cách giữa các viên đạn (giây)
    public float damage = 10f;       // Sát thương của đạn
    public float bulletSpeed = 15f;  // Tốc độ đạn bay
    public int bulletsPerShot = 1;   // Số viên đạn bắn ra cùng lúc (Shotgun bắn nhiều viên)
    public float spread = 0f;        // Độ lệch tâm (độ tỏa của đạn)
    
    public GameObject bulletPrefab;  // Hình dáng viên đạn của súng này
}