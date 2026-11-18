using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(float xForce, float yForce, float time)
    {
        StartCoroutine(KnockbackRoutine(xForce, yForce, time));
    }

    IEnumerator KnockbackRoutine(
        float xForce,
        float yForce,
        float time
    )
    {
        Player1_con p1 =
            GetComponent<Player1_con>();

        Player2_con p2 =
            GetComponent<Player2_con>();

        if (p1 != null)
            p1.lockControl = true;

        if (p2 != null)
            p2.lockControl = true;

        rb.linearVelocity =
            new Vector2(
                xForce,
                yForce
            );

        yield return new WaitForSeconds(time);

        if (p1 != null)
            p1.lockControl = false;

        if (p2 != null)
            p2.lockControl = false;
    }
}