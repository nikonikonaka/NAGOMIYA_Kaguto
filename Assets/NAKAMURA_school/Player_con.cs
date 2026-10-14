
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    private Gamepad myPad;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Joyconから1P専用コントローラーを受け取る
    public void SetPad(Gamepad pad)
    {
        myPad = pad;

        if (myPad != null)
        {
            Debug.Log("1P専用コントローラー: "
                + myPad.displayName
                + " / ID=" + myPad.deviceId);
        }
    }

    void Update()
    {
        if (myPad == null)
            return;

        moveInput = myPad.leftStick.x.ReadValue();

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            Debug.Log(
                "[1P] ID=" + myPad.deviceId
                + " / 入力=" + moveInput
            );
        }

        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        if (myPad == null)
            return;

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
