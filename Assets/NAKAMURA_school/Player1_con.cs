
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

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

        // コントローラーを受け取るまでは操作禁止
        joined = false;
        myPad = null;
        moveInput = 0f;

        // 横移動を停止
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Joycon.cs から1P用コントローラーを受け取る
    public void SetPad(Gamepad pad)
    {
        if (pad == null)
            return;

        // すでに登録済みなら、別のコントローラーに変更させない
        if (joined)
            return;

        myPad = pad;
        joined = true;
        moveInput = 0f;

        Debug.Log(
            "[1P] コントローラー登録: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        // 1P用コントローラー登録前は操作禁止
        if (!joined || myPad == null)
            return;

        // 登録された1P用コントローラーだけを読む
        moveInput = myPad.leftStick.x.ReadValue();

        // ジャンプ
        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        // 左右の向き
        if (moveInput > 0.1f)
            sr.flipX = false;
        else if (moveInput < -0.1f)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        // 登録前、またはコントローラーがない場合は横移動禁止
        if (!joined || myPad == null)
        {
            moveInput = 0f;

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(
                    0f,
                    rb.linearVelocity.y
                );
            }

            return;
        }

        // 登録した1P用コントローラーの入力だけで移動
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
        }
    }
}
