using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có để điều khiển Slider UI

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Slider healthSlider; // Kéo thanh Slider UI vào đây

    void Start()
    {
        currentHealth = maxHealth;

        // Thiết lập giá trị tối đa cho thanh máu UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player bị cào! Máu còn: " + currentHealth);

        // Cập nhật lên thanh máu UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER! Player đã hy sinh.");
        // Tạm thời đóng băng game khi chết để test
        Time.timeScale = 0f; 
        Destroy(gameObject);
    }
}