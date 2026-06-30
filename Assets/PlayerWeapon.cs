using UnityEngine;
using System.Collections.Generic;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public List<WeaponData> availableWeapons; // Danh sách các súng nhân vật đang có
    
    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;

    void Update()
    {
        if (availableWeapons.Count == 0) return;

        // 1. Click chuột trái để bắn
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + availableWeapons[currentWeaponIndex].fireRate;
        }

        // 2. Cuộn chuột hoặc bấm nút Q để đổi vũ khí
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    void Shoot()
    {
        WeaponData currentWeapon = availableWeapons[currentWeaponIndex];

        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            // Tính toán độ lệch tâm (spread)
            float currentSpread = Random.Range(-currentWeapon.spread, currentWeapon.spread);
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, 0, currentSpread);

            // Tạo đạn
            GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, bulletRotation);
            
            // Cài đặt chỉ số cho viên đạn
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetupBullet(currentWeapon.damage, currentWeapon.bulletSpeed);
            }
        }
    }

    void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Count;
        Debug.Log("Đã đổi sang súng: " + availableWeapons[currentWeaponIndex].weaponName);
    }
}