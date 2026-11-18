using UnityEngine;
using System.Collections;

public class BlowAway : MonoBehaviour
{
    [Header("êÅÇ¡îÚÇ—")]
    public float knockbackX = 20f;
    public float knockbackY = 5f;

    [Header("çSë©éûä‘")]
    public float knockbackTime = 0.3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerKnockback pk =
            collision.gameObject.GetComponent<PlayerKnockback>();

        if (pk == null)
            return;

        float dir =
            collision.transform.position.x >
            transform.position.x
            ? 1f
            : -1f;

        pk.Knockback(
            dir * knockbackX,
            knockbackY,
            knockbackTime
        );
    }
}