using UnityEngine;

public class Structure : MonoBehaviour
{
    public StructureData data;
    private float currentHealth;
    
    [Header("Production Settings")]
    public float moneyProductionRate = 3f; // Cứ 3 giây tạo ra tiền
    private float nextProductionTime;

    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float fireRate = 1f;
    private float nextFireTime;
    public GameObject bulletPrefab; // Kéo prefab viên đạn của bạn vào đây

    void Start()
    {
        if (data != null) currentHealth = data.maxHealth;
        nextProductionTime = Time.time + moneyProductionRate;
    }

    void Update()
    {
        if (data == null) return;

        switch (data.type)
        {
            case StructureType.PRODUCTION:
                HandleProduction();
                break;
            case StructureType.ATTACK:
                HandleAttack();
                break;
            case StructureType.CENTRAL:
                // Nếu pháo đài trung tâm sập thì Game Over
                break;
        }
    }

    void HandleProduction()
    {
        if (Time.time >= nextProductionTime)
        {
            // Tạm thời Debug, sau này bạn cộng vào ví tiền của Player
            Debug.Log("Nhà sản xuất tạo ra +10 Vàng!"); 
            nextProductionTime = Time.time + moneyProductionRate;
        }
    }

    void HandleAttack()
    {
        if (Time.time >= nextFireTime)
        {
            // Tìm zombie gần nhất
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            GameObject target = null;
            float shortestDistance = Mathf.Infinity;

            foreach (GameObject zombie in zombies)
            {
                float distance = Vector2.Distance(transform.position, zombie.transform.position);
                if (distance < shortestDistance && distance <= attackRange)
                {
                    shortestDistance = distance;
                    target = zombie;
                }
            }

            // Bắn zombie
            if (target != null)
            {
                Vector2 shootDir = (target.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
                
                // Tạo đạn và bắn (bạn có thể tạo một điểm bắn FirePoint trên tháp súng)
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle - 90f));
                bullet.GetComponent<Bullet>().SetupBullet(15f, 12f); // Sát thương 15, tốc độ 12

                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (data.type == StructureType.CENTRAL)
            {
                Debug.Log("PHÁO ĐÀI TRUNG TÂM SẬP! BẠN THUA.");
                Time.timeScale = 0f;
            }
            Destroy(gameObject);
        }
    }
}