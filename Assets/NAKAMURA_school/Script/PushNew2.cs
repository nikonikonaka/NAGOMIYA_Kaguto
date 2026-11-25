using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlowAway : MonoBehaviour
{
    [Header("êÅÇ¡îÚÇ—")]
    public float knockbackX = 20f;

    public float knockbackY = 5f;

    [Header("çSë©éûä‘")]
    public float knockbackTime = 0.3f;

    private HashSet<PlayerKnockback> activeTargets =
        new HashSet<PlayerKnockback>();

    private void OnCollisionEnter2D(
        Collision2D collision
    )
    {
        PlayerKnockback pk =
            collision.gameObject.GetComponent<PlayerKnockback>();

        if (pk == null)
            return;

        if (activeTargets.Contains(pk))
            return;

        float dir =
            collision.transform.position.x >
            transform.position.x
            ? 1f
            : -1f;

        activeTargets.Add(pk);

        pk.Knockback(
            dir * knockbackX,
            knockbackY,
            knockbackTime
        );

        StartCoroutine(
            ReleaseTarget(
                pk,
                knockbackTime
            )
        );
    }

    IEnumerator ReleaseTarget(
        PlayerKnockback pk,
        float time
    )
    {
        yield return new WaitForSeconds(
            time
        );

        activeTargets.Remove(pk);
    }
}