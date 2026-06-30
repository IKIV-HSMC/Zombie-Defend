using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public ZombieData zombieData;

    private float currentHealth;
    private float activeMoveSpeed;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Attack Settings")]
    public float attackSpeed = 1f;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        if (zombieData != null)
        {
            SetupZombie();
        }
    }

    void SetupZombie()
    {
        currentHealth = zombieData.maxHealth;
        activeMoveSpeed = zombieData.moveSpeed;
        transform.localScale = zombieData.localScale;

        // --- THÊM DÒNG NÀY ĐỂ ĐỒNG BỘ TỐC ĐỘ ĐÁNH TỪ DATA ---
        attackSpeed = zombieData.attackSpeed;

        if (zombieData.zombieSprite != null)
        {
            spriteRenderer.sprite = zombieData.zombieSprite;
        }
        spriteRenderer.color = zombieData.zombieColor;
        gameObject.name = zombieData.zombieName;
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * activeMoveSpeed * Time.fixedDeltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f;
        }
    }

    // Thay vì chỉ nhận amount, chúng ta thêm một tham số optional 'attacker' để biết ai tấn công
    public void TakeDamage(float amount, GameObject attacker = null)
    {
        float finalDamage = amount;

        // Nếu có kẻ tấn công và kẻ đó chính là Player
        if (attacker != null && attacker.CompareTag("Player"))
        {
            // Kiểm tra xem hệ thống PlayerStatus có tồn tại không
            if (PlayerStatus.Instance != null)
            {
                // Zombie tự nhân thêm sát thương dựa trên % buff của Player
                finalDamage = amount * (1f + PlayerStatus.Instance.damageBuffPercent);
            }
        }

        currentHealth -= finalDamage;

        if (currentHealth <= 0)
        {
            // Cộng điểm và tiền khi chết
            if (PlayerStatus.Instance != null)
            {
                PlayerStatus.Instance.AddZombieKill(10);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time >= nextAttackTime)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(zombieData.damage);
                    nextAttackTime = Time.time + attackSpeed;
                }
            }
            else if (collision.gameObject.GetComponent<Structure>() != null)
            {
                Structure structure = collision.gameObject.GetComponent<Structure>();
                if (structure != null)
                {
                    structure.TakeDamage(zombieData.damage);
                    nextAttackTime = Time.time + attackSpeed;
                }
            }
        }
    }
}