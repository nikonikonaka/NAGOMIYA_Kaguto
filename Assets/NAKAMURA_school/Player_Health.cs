using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("HPバー")]
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            KO();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void KO()
    {
        Debug.Log(gameObject.name + " KO!");

        // プレイヤー操作停止
        MonoBehaviour movement = GetComponent<Player1_con>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        // Resultシーンへ移動
        SceneManager.LoadScene("Result");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
