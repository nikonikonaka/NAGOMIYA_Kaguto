
using UnityEngine;
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
}
