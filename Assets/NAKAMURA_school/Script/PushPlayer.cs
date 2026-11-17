using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBounce : MonoBehaviour
{
    [Header("íµÇÀï‘Ç∑óÕ")]
    public float horizontalForce = 15f;

    public float verticalForce = 10f;

    [Header("êÅÇ¡îÚÇŒÇµéûä‘")]
    public float bounceTime = 0.4f;

    [Header("ê›íË")]
    public bool bounceOnlyOnce = true;

    private HashSet<Rigidbody2D> hitPlayers =
        new HashSet<Rigidbody2D>();

    private HashSet<Rigidbody2D> bouncingPlayers =
        new HashSet<Rigidbody2D>();

    void OnEnable()
    {
        hitPlayers.Clear();
        bouncingPlayers.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Transform targetPlayer =
            FindPlayerRoot(other.transform);

        if (targetPlayer == null)
            return;

        Transform myPlayer =
            FindPlayerRoot(transform);

        if (myPlayer != null &&
            targetPlayer == myPlayer)
            return;

        Rigidbody2D targetRb =
            targetPlayer.GetComponent<Rigidbody2D>();

        if (targetRb == null)
        {
            targetRb =
                targetPlayer.GetComponentInChildren<Rigidbody2D>();
        }

        if (targetRb == null)
            return;

        if (targetRb.bodyType != RigidbodyType2D.Dynamic)
            return;

        if (bouncingPlayers.Contains(targetRb))
            return;

        if (bounceOnlyOnce &&
            hitPlayers.Contains(targetRb))
            return;

        hitPlayers.Add(targetRb);
        bouncingPlayers.Add(targetRb);

        float attackerX =
            myPlayer != null
            ? myPlayer.position.x
            : transform.position.x;

        float dir =
            Mathf.Sign(
                targetRb.position.x - attackerX
            );

        if (dir == 0f)
        {
            SpriteRenderer sr =
                myPlayer != null
                ? myPlayer.GetComponentInChildren<SpriteRenderer>()
                : null;

            dir =
                (sr != null && sr.flipX)
                ? -1f
                : 1f;
        }

        StartCoroutine(
            BouncePlayer(
                targetPlayer,
                targetRb,
                dir
            )
        );
    }

    IEnumerator BouncePlayer(
        Transform targetPlayer,
        Rigidbody2D targetRb,
        float dir)
    {
        if (targetRb == null)
            yield break;

        Player_test_con1 testPlayer1 =
            targetPlayer.GetComponentInChildren<Player_test_con1>();

        Player_test_con2 testPlayer2 =
            targetPlayer.GetComponentInChildren<Player_test_con2>();

        Player1_con p1 =
            targetPlayer.GetComponentInChildren<Player1_con>();

        Player2_con p2 =
            targetPlayer.GetComponentInChildren<Player2_con>();

        bool test1Enabled =
            testPlayer1 != null &&
            testPlayer1.enabled;

        bool test2Enabled =
            testPlayer2 != null &&
            testPlayer2.enabled;

        bool p1Knockback =
            p1 != null &&
            p1.knockback;

        bool p2Knockback =
            p2 != null &&
            p2.knockback;

        if (testPlayer1 != null)
            testPlayer1.enabled = false;

        if (testPlayer2 != null)
            testPlayer2.enabled = false;

        if (p1 != null)
        {
            p1.knockback = true;
        }

        if (p2 != null)
        {
            p2.knockback = true;
        }

        targetRb.linearVelocity =
            new Vector2(
                dir * horizontalForce,
                verticalForce
            );

        float timer = 0f;

        while (timer < bounceTime)
        {
            if (targetRb == null)
                yield break;

            targetRb.linearVelocity =
                new Vector2(
                    dir * horizontalForce,
                    targetRb.linearVelocity.y
                );

            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        hitPlayers.Remove(targetRb);
        bouncingPlayers.Remove(targetRb);

        if (testPlayer1 != null)
            testPlayer1.enabled =
                test1Enabled;

        if (testPlayer2 != null)
            testPlayer2.enabled =
                test2Enabled;

        if (p1 != null)
        {
            p1.knockback =
                p1Knockback;
        }

        if (p2 != null)
        {
            p2.knockback =
                p2Knockback;
        }
    }

    Transform FindPlayerRoot(
        Transform current)
    {
        while (current != null)
        {
            if (current.CompareTag("Player"))
                return current;

            current = current.parent;
        }

        return null;
    }
}