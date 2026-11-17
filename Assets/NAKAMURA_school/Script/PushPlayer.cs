using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBounce : MonoBehaviour
{
    [Header("跳ね返す力")]
    public float horizontalForce = 15f;

    public float verticalForce = 10f;

    [Header("吹っ飛ばし時間")]
    public float bounceTime = 0.4f;

    [Header("設定")]
    public bool bounceOnlyOnce = true;

    private HashSet<Rigidbody2D> hitPlayers =
        new HashSet<Rigidbody2D>();

    private HashSet<Rigidbody2D> bouncingPlayers =
        new HashSet<Rigidbody2D>();

    void OnEnable()
    {
        Debug.Log("[Bounce] OnEnable");

        hitPlayers.Clear();
        bouncingPlayers.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            "[Bounce] Trigger : " +
            other.name
        );

        Transform targetPlayer =
            FindPlayerRoot(other.transform);

        if (targetPlayer == null)
        {
            Debug.Log(
                "[Bounce] PlayerRootなし"
            );
            return;
        }

        Debug.Log(
            "[Bounce] Target = " +
            targetPlayer.name
        );

        Transform myPlayer =
            FindPlayerRoot(transform);

        if (myPlayer != null &&
            targetPlayer == myPlayer)
        {
            Debug.Log(
                "[Bounce] 自分自身なので無視"
            );
            return;
        }

        Rigidbody2D targetRb =
            targetPlayer.GetComponent<Rigidbody2D>();

        if (targetRb == null)
        {
            targetRb =
                targetPlayer.GetComponentInChildren<Rigidbody2D>();
        }

        if (targetRb == null)
        {
            Debug.Log(
                "[Bounce] Rigidbody2Dなし"
            );
            return;
        }

        Debug.Log(
            "[Bounce] Rigidbody = " +
            targetRb.name
        );

        if (targetRb.bodyType != RigidbodyType2D.Dynamic)
        {
            Debug.Log(
                "[Bounce] Dynamicではない"
            );
            return;
        }

        if (bouncingPlayers.Contains(targetRb))
        {
            Debug.Log(
                "[Bounce] すでに吹っ飛び中"
            );
            return;
        }

        if (bounceOnlyOnce &&
            hitPlayers.Contains(targetRb))
        {
            Debug.Log(
                "[Bounce] 既にヒット済み"
            );
            return;
        }

        hitPlayers.Add(targetRb);
        bouncingPlayers.Add(targetRb);

        float attackerX =
            myPlayer != null
            ? myPlayer.position.x
            : transform.position.x;

        float dir =
            Mathf.Sign(
                targetRb.position.x -
                attackerX
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

        Debug.Log(
            "[Bounce] dir = " + dir
        );

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
        Debug.Log(
            "[Bounce] Coroutine開始 : " +
            targetPlayer.name
        );

        if (targetRb == null)
        {
            Debug.Log(
                "[Bounce] Rigidbody消失"
            );
            yield break;
        }

        RigidbodyConstraints2D originalConstraints =
            targetRb.constraints;

        Debug.Log(
            "[Bounce] 元Constraints = " +
            originalConstraints
        );

        Player_test_con1 testPlayer1 =
            targetPlayer.GetComponentInChildren<Player_test_con1>();

        Player_test_con2 testPlayer2 =
            targetPlayer.GetComponentInChildren<Player_test_con2>();

        Player1_con p1 =
            targetPlayer.GetComponentInChildren<Player1_con>();

        Player2_con p2 =
            targetPlayer.GetComponentInChildren<Player2_con>();

        Debug.Log(
            "[Bounce] Player_test_con1 = " +
            (testPlayer1 != null)
        );

        Debug.Log(
            "[Bounce] Player_test_con2 = " +
            (testPlayer2 != null)
        );

        Debug.Log(
            "[Bounce] Player1_con = " +
            (p1 != null)
        );

        Debug.Log(
            "[Bounce] Player2_con = " +
            (p2 != null)
        );

        if (p2 != null)
        {
            Debug.Log(
                "[Bounce] Player2_con取得先 = " +
                p2.gameObject.name
            );
        }

        bool test1Enabled =
            testPlayer1 != null &&
            testPlayer1.enabled;

        bool test2Enabled =
            testPlayer2 != null &&
            testPlayer2.enabled;

        bool p1Lock =
            p1 != null &&
            p1.lockControl;

        bool p1Knockback =
            p1 != null &&
            p1.knockback;

        bool p2Lock =
            p2 != null &&
            p2.lockControl;

        bool p2Knockback =
            p2 != null &&
            p2.knockback;

        Debug.Log(
            "[Bounce] 保存状態 p1 lock=" +
            p1Lock +
            " knockback=" +
            p1Knockback
        );

        Debug.Log(
            "[Bounce] 保存状態 p2 lock=" +
            p2Lock +
            " knockback=" +
            p2Knockback
        );

        if (testPlayer1 != null)
        {
            testPlayer1.enabled = false;
            Debug.Log(
                "[Bounce] test1停止"
            );
        }

        if (testPlayer2 != null)
        {
            testPlayer2.enabled = false;
            Debug.Log(
                "[Bounce] test2停止"
            );
        }

        if (p1 != null)
        {
            p1.lockControl = true;
            p1.knockback = true;

            Debug.Log(
                "[Bounce] p1吹っ飛び開始"
            );
        }

        if (p2 != null)
        {
            p2.lockControl = true;
            p2.knockback = true;

            Debug.Log(
                "[Bounce] p2吹っ飛び開始"
            );
        }

        targetRb.constraints &=
            ~RigidbodyConstraints2D.FreezePositionX;

        targetRb.linearVelocity =
            Vector2.zero;

        Debug.Log(
            "[Bounce] Velocity初期化"
        );

        Vector2 force =
            new Vector2(
                dir * horizontalForce,
                verticalForce
            );

        Debug.Log(
            "[Bounce] Force = " +
            force
        );

        targetRb.AddForce(
            force,
            ForceMode2D.Impulse
        );

        Debug.Log(
            "[Bounce] AddForce完了"
        );

        yield return new WaitForSeconds(
            bounceTime
        );

        Debug.Log(
            "[Bounce] 復帰処理開始"
        );

        hitPlayers.Remove(targetRb);
        bouncingPlayers.Remove(targetRb);

        if (targetRb != null)
        {
            targetRb.constraints =
                originalConstraints;

            Debug.Log(
                "[Bounce] Constraints復元"
            );
        }

        if (testPlayer1 != null)
        {
            testPlayer1.enabled =
                test1Enabled;

            Debug.Log(
                "[Bounce] test1復帰"
            );
        }

        if (testPlayer2 != null)
        {
            testPlayer2.enabled =
                test2Enabled;

            Debug.Log(
                "[Bounce] test2復帰"
            );
        }

        if (p1 != null)
        {
            p1.lockControl =
                p1Lock;

            p1.knockback =
                p1Knockback;

            Debug.Log(
                "[Bounce] p1復帰"
            );
        }

        if (p2 != null)
        {
            p2.lockControl =
                p2Lock;

            p2.knockback =
                p2Knockback;

            Debug.Log(
                "[Bounce] p2復帰"
            );
        }

        Debug.Log(
            "[Bounce] 完全終了 : " +
            targetPlayer.name
        );
    }

    Transform FindPlayerRoot(
        Transform current)
    {
        while (current != null)
        {
            if (
                current.CompareTag(
                    "Player"
                )
            )
            {
                Debug.Log(
                    "[Bounce] PlayerRoot発見 : " +
                    current.name
                );

                return current;
            }

            current =
                current.parent;
        }

        return null;
    }
}
/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBounce : MonoBehaviour
{
    [Header("跳ね返す力")]
    public float horizontalForce = 15f;

    public float verticalForce = 10f;

    [Header("吹っ飛ばし時間")]
    public float bounceTime = 0.4f;

    [Header("設定")]
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

        Debug.Log(
            "跳ね返し開始 : " +
            targetPlayer.name +
            " dir=" +
            dir
        );

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

        bool p1Lock =
            p1 != null &&
            p1.lockControl;

        bool p1Knockback =
            p1 != null &&
            p1.knockback;

        bool p2Lock =
            p2 != null &&
            p2.lockControl;

        bool p2Knockback =
            p2 != null &&
            p2.knockback;

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

        targetRb.constraints &=
            ~RigidbodyConstraints2D.FreezePositionX;

        // AddForce前に速度初期化
        targetRb.linearVelocity = Vector2.zero;

        Vector2 force =
            new Vector2(
                dir * horizontalForce,
                verticalForce
            );

        targetRb.AddForce(
            force,
            ForceMode2D.Impulse
        );

        Debug.Log(
            "AddForce : " +
            force +
            " target=" +
            targetPlayer.name
        );

        yield return new WaitForSeconds(
            bounceTime
        );

        hitPlayers.Remove(targetRb);
        bouncingPlayers.Remove(targetRb);

        if (targetRb != null)
        {
            targetRb.constraints =
                originalConstraints;
        }

        if (testPlayer1 != null)
            testPlayer1.enabled =
                test1Enabled;

        if (testPlayer2 != null)
            testPlayer2.enabled =
                test2Enabled;

        if (p1 != null)
        {
            p1.lockControl =
                p1Lock;

            p1.knockback =
                p1Knockback;
        }

        if (p2 != null)
        {
            p2.lockControl =
                p2Lock;

            p2.knockback =
                p2Knockback;
        }

        Debug.Log(
            "跳ね返し終了 : " +
            targetPlayer.name
        );
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
}*/