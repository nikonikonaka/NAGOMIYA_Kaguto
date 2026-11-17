using UnityEngine;
using System.Collections;

public class PlayerNew : MonoBehaviour
{
    [Header("êÅÇ¡îÚÇ—")]
    public float knockbackX = 12f;
    public float knockbackY = 4f;
    public float gravityRestoreTime = 0.15f;

    [Header("ëΩèdÉqÉbÉgñhé~")]
    public float invincibleTime = 0.2f;

    private Rigidbody2D rb;
    private Player2_con player2;

    private float defaultGravity;
    private bool isKnockback;
    private bool canHit = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player2 = GetComponent<Player2_con>();

        if (rb != null)
        {
            defaultGravity = rb.gravityScale;
        }
    }

    /// <summary>
    /// dirX
    /// 1 = âEÇ÷êÅÇ¡îÚÇ‘
    /// -1 = ç∂Ç÷êÅÇ¡îÚÇ‘
    /// </summary>
    public void KnockBack(float dirX)
    {
        if (!canHit) return;
        if (rb == null) return;

        StartCoroutine(KnockBackRoutine(dirX));
    }

    private IEnumerator KnockBackRoutine(float dirX)
    {
        canHit = false;
        isKnockback = true;

        if (player2 != null)
        {
            player2.knockback = true;
        }

        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            dirX * knockbackX,
            knockbackY
        );

        yield return new WaitForSeconds(gravityRestoreTime);

        rb.gravityScale = defaultGravity;

        isKnockback = false;

        if (player2 != null)
        {
            player2.EndKnockback();
        }

        yield return new WaitForSeconds(invincibleTime);

        canHit = true;
    }

    public bool IsKnockback()
    {
        return isKnockback;
    }
}