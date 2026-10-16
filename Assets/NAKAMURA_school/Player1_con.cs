using UnityEngine;
using UnityEngine.InputSystem;

public class Player1_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [Header("攻撃")]
    public float dashSpeed = 12f;
    public float dashTime = 1.0f; // 1秒突進
    public int attackDamage = 10;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private bool airDashUsed = false;

    private Gamepad myPad;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;
    private bool joined = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        joined = false;
        myPad = null;
        moveInput = 0f;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Joycon.cs から1P用コントローラーを受け取る
    public void SetPad(Gamepad pad)
    {
        if (pad == null) return;
        if (joined) return;

        myPad = pad;
        joined = true;

        Debug.Log(
            "[1P] コントローラー登録: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        if (!joined || myPad == null)
            return;

        // 左スティック
        moveInput = myPad.leftStick.x.ReadValue();

        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;

            // 空中突進回数リセット
            airDashUsed = false;
        }

        if (myPad.buttonEast.wasPressedThisFrame && !isAttacking)
        {
            // 地上なら何回でも
            if (isGrounded)
            {
                isAttacking = true;
                attackTimer = dashTime;
            }
            // 空中なら1回だけ
            else if (!airDashUsed)
            {
                isAttacking = true;
                attackTimer = dashTime;
                airDashUsed = true;
            }
        }

        // 向き
        if (moveInput > 0.1f)
        {
            sr.flipX = false;
        }
        else if (moveInput < -0.1f)
        {
            sr.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (!joined || myPad == null)
        {
            return;
        }

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

        // 通常移動 1pコントローラー専用
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