using UnityEngine;

public class Stand : MonoBehaviour
{
    public float downAngle = 60f;
    public float downDelay = 0.7f;
    public float standSpeed = 300f; // 起き上がり速度

    private Rigidbody2D rb;
    private Player1_con playerCon;
    private Player_Health hp;

    private bool isDown = false;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCon = GetComponent<Player1_con>();
        hp = GetComponent<Player_Health>();
    }

    void Update()
    {
        // HP0なら停止
        if (hp != null && hp.GetCurrentHealth() <= 0)
        {
            if (playerCon != null)
                playerCon.enabled = false;

            return;
        }

        float z = transform.eulerAngles.z;

        if (z > 180f)
            z -= 360f;

        // ダウン判定
        if (!isDown && Mathf.Abs(z) > downAngle)
        {
            isDown = true;
            timer = downDelay;

            if (playerCon != null)
                playerCon.enabled = false;
        }

        if (!isDown)
            return;

        // ダウン時間
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        // ゆっくり起き上がる
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.identity,
            standSpeed * Time.deltaTime
        );

        // 起き上がり中は回転速度を弱める
        rb.angularVelocity *= 0.9f;

        // 完全に起きた
        if (Quaternion.Angle(transform.rotation, Quaternion.identity) < 3f)
        {
            transform.rotation = Quaternion.identity;

            rb.angularVelocity = 0f;
            rb.linearVelocity *= 0.5f;

            if (playerCon != null)
                playerCon.enabled = true;

            isDown = false;
        }
    }
}