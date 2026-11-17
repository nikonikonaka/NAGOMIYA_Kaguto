using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBounce : MonoBehaviour
{
    [Header("吹っ飛び設定")]
    public float knockbackX = 30f;

    public float knockbackY = 4f;

    [Header("吹っ飛び中の重力")]
    public float knockbackGravity = 5f;

    [Header("吹っ飛ばし時間")]
    public float bounceTime = 0.5f;

    [Header("設定")]
    public bool bounceOnlyOnce = true;

    private HashSet<Rigidbody2D> hitPlayers =
        new HashSet<Rigidbody2D>();

    // 現在吹っ飛び中のプレイヤー
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

        // 既に吹っ飛び中なら無視
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

        RigidbodyConstraints2D originalConstraints =
            targetRb.constraints;

        float originalGravity =
            targetRb.gravityScale;

        Player_test_con1 testPlayer1 =
            targetPlayer.GetComponentInChildren<Player_test_con1>();

        Player_test_con2 testPlayer2 =
            targetPlayer.GetComponentInChildren<Player_test_con2>();

        Player1_con p1 =
            targetPlayer.GetComponentInChildren<Player1_con>();

        Player2_con p2 =
            targetPlayer.GetComponentInChildren<Player2_con>();

        if (testPlayer1 != null)
            testPlayer1.enabled = false;

        if (testPlayer2 != null)
            testPlayer2.enabled = false;

        if (p1 != null)
        {
            p1.lockControl = true;
            p1.knockback = true;
        }

        if (p2 != null)
        {
            p2.lockControl = true;
            p2.knockback = true;
        }

        targetRb.gravityScale =
            knockbackGravity;

        targetRb.linearVelocity =
            new Vector2(
                dir * knockbackX,
                knockbackY
            );

        yield return new WaitForSeconds(
            bounceTime
        );

        hitPlayers.Remove(targetRb);
        bouncingPlayers.Remove(targetRb);

        if (targetRb != null)
        {
            // 横速度をリセット
            targetRb.linearVelocity =
                new Vector2(
                    0f,
                    targetRb.linearVelocity.y
                );

            targetRb.constraints =
                originalConstraints;

            targetRb.gravityScale =
                originalGravity;
        }

        if (testPlayer1 != null)
            testPlayer1.enabled = true;

        if (testPlayer2 != null)
            testPlayer2.enabled = true;

        if (p1 != null)
        {
            p1.knockback = false;
            p1.lockControl = false;
        }

        if (p2 != null)
        {
            p2.knockback = false;
            p2.lockControl = false;
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