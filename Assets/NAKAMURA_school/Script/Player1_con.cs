using UnityEngine;
using UnityEngine.InputSystem;

public class Player1_con : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    [Header("çUåÇ")]
    public float dashSpeed = 6f;
    public float dashTime = 1.0f;
    public int attackDamage = 10;

    [Header("SE")]
    public AudioClip dashSE;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private bool airDashUsed = false;

    [HideInInspector]
    public bool lockControl = false;

    [HideInInspector]
    public bool knockback = false;

    public Gamepad myPad;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private float moveInput;
    private bool isGrounded;
    private bool joined = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        joined = false;
        myPad = null;
        moveInput = 0f;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetPad(Gamepad pad)
    {
        if (pad == null) return;
        if (joined) return;

        myPad = pad;
        joined = true;

        Debug.Log(
            "[1P] ÉRÉìÉgÉçÅ[ÉâÅ[ìoò^: " +
            myPad.displayName +
            " / ID=" + myPad.deviceId
        );
    }

    void Update()
    {
        if (!joined || myPad == null)
            return;

        if (lockControl)
        {
            moveInput = 0f;
            return;
        }

        moveInput = myPad.leftStick.x.ReadValue();

        if (myPad.buttonSouth.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
            airDashUsed = false;
        }

        if (moveInput > 0.1f)
        {
            sr.flipX = false;
        }
        else if (moveInput < -0.1f)
        {
            sr.flipX = true;
        }
    }

    public bool IsFacingLeft()
    {
        return sr != null && sr.flipX;
    }

    public void EndKnockback()
    {
        knockback = false;
    }

    void FixedUpdate()
    {
        if (!joined || myPad == null)
            return;

        if (lockControl)
            return;

        if (knockback)
            return;

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

        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        foreach (ContactPoint2D point in col.contacts)
        {
            if (point.normal.y > 0.5f)
            {
                isGrounded = true;
                airDashUsed = false;
                return;
            }
        }

        isGrounded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}