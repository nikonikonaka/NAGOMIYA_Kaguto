using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushPower = 2f;

    private Rigidbody2D rb;

   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                gameObject.name +
                " ‚É Rigidbody2D ‚ª‚ ‚è‚Ü‚¹‚ñ"
            );
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D otherRb =
            collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRb == null)
            return;

        Vector2 dir =
            (otherRb.position - rb.position).normalized;

        rb.MovePosition(
            rb.position - dir * pushPower * Time.fixedDeltaTime
        );

        // “®‚«‚ðƒŠƒZƒbƒg
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}