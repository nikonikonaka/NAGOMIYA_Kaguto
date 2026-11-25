using UnityEngine;

public class PlayerNoPush : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        rb.position = new Vector2(
            transform.position.x,
            rb.position.y
        );
    }
}