using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHealth = 100;

    // 全Player_Healthで共有
    private static int currentHealth;

    [Header("HPバー")]
    public Slider healthBar;

    [Header("軽減設定")]
    public bool damageReduce = false;

    [Range(0f, 1f)]
    public float reduceRate = 0.5f;

    private static bool initialized = false;

    void Start()
    {
        if (!initialized)
        {
            currentHealth = maxHealth;
            initialized = true;
        }

        UpdateHealthBar();
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHealthBar();
    }

    public void EnableGuard()
    {
        damageReduce = true;
    }

    public void DisableGuard()
    {
        damageReduce = false;
    }

    public void TakeDamage(int damage)
    {
        if (damageReduce)
        {
            damage = Mathf.RoundToInt(
                damage * reduceRate
            );
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        Debug.Log(
            gameObject.name +
            " Damage : " +
            damage +
            " HP : " +
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
            p1.enabled = false;

        Player2_con p2 =
            GetComponent<Player2_con>();

        if (p2 != null)
            p2.enabled = false;

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
/*using UnityEngine;
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

    // 外部からHPバーを設定する
    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthBar();

        if (currentHealth <= 0)
            KO();
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

        Player1_con p1 = GetComponent<Player1_con>();
        if (p1 != null)
            p1.enabled = false;

        Player2_con p2 = GetComponent<Player2_con>();
        if (p2 != null)
            p2.enabled = false;

        SceneManager.LoadScene("Result");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthBar();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}*/