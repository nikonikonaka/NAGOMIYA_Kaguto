using UnityEngine;
using UnityEngine.InputSystem;

public class Player_test_con1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [Header("攻撃")]
    public float dashSpeed = 12f;
    public float dashTime = 1.0f;
    public int attackDamage = 10;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private bool airDashUsed = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 左右移動
        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.dKey.isPressed)
            moveInput = 1f;

        // ジャンプ　w
        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        // 突進 (スペースキー)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isAttacking)
        {
            if (isGrounded)
            {
                isAttacking = true;
                attackTimer = dashTime;
            }
            else if (!airDashUsed)
            {
                isAttacking = true;
                attackTimer = dashTime;
                airDashUsed = true;
            }
        }

        // 向き
        if (moveInput > 0.1f)
            sr.flipX = false;
        else if (moveInput < -0.1f)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        // 突進中
        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            float dir = sr.flipX ? -1f : 1f;

            rb.linearVelocity = new Vector2(
                dir * dashSpeed,
                rb.linearVelocity.y
            );

            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }

            return;
        }

        // 通常移動
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            airDashUsed = false;
        }

        // 攻撃ヒット
        if (isAttacking && col.gameObject.CompareTag("Player"))
        {
            Player_Health hp =
                col.gameObject.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(attackDamage);
                isAttacking = false;
            }
        }
    }
}