
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [Header("攻撃")]
    public float dashSpeed = 12f;
    public float dashTime = 1.0f;
    public int attackDamage = 10;

    [Header("SE")]
    public AudioClip dashSE;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private bool airDashUsed = false;

    [HideInInspector]
    public bool joined = false;

    [HideInInspector]
    public Gamepad myPad;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        Debug.Log("[2P] Awake: " + gameObject.name);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetPad(Gamepad pad)
    {
        myPad = pad;
        joined = true;

        Debug.Log(
            "[2P] コントローラー割り当て: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        if (!joined || myPad == null)
            return;

        moveInput = myPad.leftStick.x.ReadValue();

        // ジャンプ
        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
            airDashUsed = false;
        }

        // Bボタンで突進
        if (myPad.buttonEast.wasPressedThisFrame && !isAttacking)
        {
            // 地上
            if (isGrounded)
            {
                isAttacking = true;
                attackTimer = dashTime;

                if (dashSE != null && audioSource != null)
                {
                    audioSource.PlayOneShot(dashSE);
                }
            }
            // 空中は1回だけ
            else if (!airDashUsed)
            {
                isAttacking = true;
                attackTimer = dashTime;
                airDashUsed = true;

                if (dashSE != null && audioSource != null)
                {
                    audioSource.PlayOneShot(dashSE);
                }
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
        if (!joined || myPad == null)
            return;

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
        if (col.gameObject.CompareTag("Ground") ||
            col.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
            airDashUsed = false;
        }

        // 突進中に相手へダメージ
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

/*using UnityEngine;  //2026 10 19  
using UnityEngine.InputSystem;

public class Player2_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [Header("攻撃")]
    public float dashSpeed = 12f;
    public float dashTime = 1.0f;
    public int attackDamage = 10;

    private bool isAttacking = false;
    private float attackTimer = 0f;

    [HideInInspector]
    public bool joined = false;

    [HideInInspector]
    public Gamepad myPad;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Debug.Log("[2P] Awake: " + gameObject.name);
    }

    public void SetPad(Gamepad pad)
    {
        myPad = pad;
        joined = true;

        Debug.Log(
            "[2P] コントローラー割り当て: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        if (!joined || myPad == null)
            return;

        moveInput = myPad.leftStick.x.ReadValue();

        // ジャンプ(A)
        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        // Bボタンで突進
        if (myPad.buttonEast.wasPressedThisFrame && !isAttacking)
        {
            isAttacking = true;
            attackTimer = dashTime;
        }

        // 向き
        if (moveInput > 0.1f)
            sr.flipX = false;
        else if (moveInput < -0.1f)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        if (!joined || myPad == null)
            return;

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
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
        
        }
        else
        {
            isGrounded = false;
        }

        // 突進中に相手へダメージ
        if (isAttacking && col.gameObject.CompareTag("Player"))
        {
            Player_Health hp =
                col.gameObject.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(attackDamage);
            }
        }
    }
}


*/

/*using UnityEngine;
using UnityEngine.InputSystem;

public class Player2_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [HideInInspector]
    public bool joined = false;

    [HideInInspector]
    public Gamepad myPad;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Debug.Log("[2P] Awake: " + gameObject.name);
    }

    public void SetPad(Gamepad pad)
    {
        myPad = pad;
        joined = true;

        Debug.Log(
            "[2P] コントローラー割り当て: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        if (!joined || myPad == null)
            return;

        // 2P専用コントローラーの入力
        moveInput = myPad.leftStick.x.ReadValue();

        // 入力値の確認（入力があるときだけ表示）
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Debug.Log(
                "[2P] 左スティック入力: ID=" +
                myPad.deviceId +
                " / 値=" + moveInput
            );
        }

        // ジャンプ
        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            Debug.Log("[2P] ジャンプ入力");

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        // 向き変更
        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        if (!joined || myPad == null)
            return;

        // 横移動
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );

        // 実際の移動速度を確認（入力があるときだけ）
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Debug.Log(
                "[2P] 移動処理: " +
                gameObject.name +
                " / 速度=" + rb.linearVelocity.x
            );
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("[2P] 地面に接触");
        }
    }
}*/
