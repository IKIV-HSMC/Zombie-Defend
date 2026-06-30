using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float speed;

    public void SetupBullet(float dmg, float spd)
    {
        damage = dmg;
        speed = spd;
        // Đạn tự động bay về phía trước (trục Y hướng lên của viên đạn)
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * speed; 
        // Tự hủy sau 3 giây nếu không trúng gì để tránh nặng máy
        Destroy(gameObject, 3f); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Phải là ZombieController chứ không phải ZombieBase cũ nhé bạn
        ZombieController zombie = collision.GetComponent<ZombieController>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}