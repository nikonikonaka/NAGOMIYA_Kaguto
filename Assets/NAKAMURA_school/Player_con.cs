using UnityEngine;

public class Player_con : MonoBehaviour
{
    public int joystickNumber = 1;

    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("J" + joystickNumber + "_Horizontal");

        if (Mathf.Abs(move) < 0.3f)
            move = 0;

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );

        // Aボタンでジャンプ
        if (Input.GetKeyDown("joystick " + joystickNumber + " button 0")
            && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            isGrounded = false;
        }

        if (move > 0)
            sr.flipX = false;
        else if (move < 0)
            sr.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}