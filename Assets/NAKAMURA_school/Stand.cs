using UnityEngine;

public class Stand : MonoBehaviour
{
    public float downAngle = 60f;      // ダウン判定角度
    public float standForce = 50f;     // 起き上がる力
    public float downDelay = 0.7f;     // 寝ている時間

    private Rigidbody2D rb;
    private Player1_con playerCon;

    private bool isDown;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCon = GetComponent<Player1_con>();
    }

    void Update()
    {
        float z = transform.eulerAngles.z;

        if (z > 180f)
            z -= 360f;

        // ダウン
        if (!isDown && Mathf.Abs(z) > downAngle)
        {
            isDown = true;
            timer = downDelay;

            if (playerCon != null)
                playerCon.enabled = false;

            rb.freezeRotation = false;
        }

        if (isDown)
        {
            // 少し寝る
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }

            // ゆっくり起き上がる
            rb.AddTorque(-z * standForce * Time.deltaTime);

            // ほぼ立った
            if (Mathf.Abs(z) < 5f)
            {
                rb.angularVelocity = 0f;
                transform.rotation = Quaternion.identity;

                rb.freezeRotation = true;

                if (playerCon != null)
                    playerCon.enabled = true;

                isDown = false;
            }
        }
    }
}