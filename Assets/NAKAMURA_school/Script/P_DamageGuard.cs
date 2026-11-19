using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Health_Guard : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("HPバー")]
    public Slider healthBar;

    [Header("ダメージ倍率")]
    [Range(0f, 1f)]
    public float reduceRate = 0.5f;
    // 1.0 = 軽減なし
    // 0.5 = 半減
    // 0.1 = 90%軽減
    // 0.0 = ダメージ0

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        int finalDamage =
            Mathf.RoundToInt(damage * reduceRate);

        currentHealth -= finalDamage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        Debug.Log(
            "ダメージ : " +
            finalDamage +
            " / HP : " +
            currentHealth
        );

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

        Player1_con p1 =
            GetComponent<Player1_con>();

        if (p1 != null)
        {
            p1.enabled = false;
        }

        Player2_con p2 =
            GetComponent<Player2_con>();

        if (p2 != null)
        {
            p2.enabled = false;
        }

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

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}