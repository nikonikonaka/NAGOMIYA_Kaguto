
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

    void OnEnable()
    {
        hitPlayers.Clear();
    }

    // Box Collider 2Dに接触したとき
    void OnTriggerEnter2D(Collider2D other)
    {
        Transform targetPlayer = FindPlayerRoot(other.transform);

        if (targetPlayer == null)
            return;

        // 攻撃者自身を取得
        Transform myPlayer = FindPlayerRoot(transform);

        // 自分自身には当てない
        if (myPlayer != null && targetPlayer == myPlayer)
            return;

        // 相手のRigidbody2Dを取得
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

        // 同じ相手への重複判定を防ぐ
        if (bounceOnlyOnce && hitPlayers.Contains(targetRb))
            return;

        hitPlayers.Add(targetRb);

        // 攻撃者と相手の位置から方向を決定
        float attackerX = myPlayer != null
            ? myPlayer.position.x
            : transform.position.x;

        float dir = Mathf.Sign(
            targetRb.position.x - attackerX
        );

        if (dir == 0f)
        {
            SpriteRenderer attackerSr =
                myPlayer != null
                ? myPlayer.GetComponentInChildren<SpriteRenderer>()
                : null;

            dir = attackerSr != null && attackerSr.flipX
                ? -1f
                : 1f;
        }

        Debug.Log(
            "跳ね返し開始: " +
            targetPlayer.name +
            " / 横方向=" + dir
        );

        StartCoroutine(
            BouncePlayer(targetPlayer, targetRb, dir)
        );
    }

    // 吹っ飛ばし処理
    IEnumerator BouncePlayer(
        Transform targetPlayer,
        Rigidbody2D targetRb,
        float dir)
    {
        if (targetRb == null)
            yield break;

        // Rigidbody2Dの設定を保存
        RigidbodyConstraints2D originalConstraints =
            targetRb.constraints;

        // X方向の固定を解除
        targetRb.constraints &=
            ~RigidbodyConstraints2D.FreezePositionX;

        // 移動スクリプトを取得
        Player_test_con1 testPlayer1 =
            targetPlayer.GetComponentInChildren<Player_test_con1>();

        Player_test_con2 testPlayer2 =
            targetPlayer.GetComponentInChildren<Player_test_con2>();

        Player1_con p1 =
            targetPlayer.GetComponentInChildren<Player1_con>();

        Player2_con p2 =
            targetPlayer.GetComponentInChildren<Player2_con>();

        // 元の有効状態を保存
        bool test1Enabled =
            testPlayer1 != null && testPlayer1.enabled;

        bool test2Enabled =
            testPlayer2 != null && testPlayer2.enabled;

        // 元の操作ロック状態を保存
        bool p1Lock = p1 != null && p1.lockControl;
        bool p1Knockback = p1 != null && p1.knockback;

        bool p2Lock = p2 != null && p2.lockControl;
        bool p2Knockback = p2 != null && p2.knockback;

        // 吹っ飛ばし中は移動スクリプトを停止
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

        // 以前の速度をリセット
        targetRb.linearVelocity = Vector2.zero;

        // 横方向＋上方向にAddForce
        Vector2 force = new Vector2(
            dir * horizontalForce,
            verticalForce
        );

        targetRb.AddForce(
            force,
            ForceMode2D.Impulse
        );

        Debug.Log(
            "AddForce実行: " +
            force +
            " / 対象=" + targetPlayer.name
        );

        // 吹っ飛ばし中は操作停止
        yield return new WaitForSeconds(bounceTime);

        // Rigidbody設定を元に戻す
        if (targetRb != null)
        {
            targetRb.constraints = originalConstraints;
        }

        // 移動スクリプトを元に戻す
        if (testPlayer1 != null)
            testPlayer1.enabled = test1Enabled;

        if (testPlayer2 != null)
            testPlayer2.enabled = test2Enabled;

        // 操作ロックを元に戻す
        if (p1 != null)
        {
            p1.lockControl = p1Lock;
            p1.knockback = p1Knockback;
        }

        if (p2 != null)
        {
            p2.lockControl = p2Lock;
            p2.knockback = p2Knockback;
        }

        Debug.Log("跳ね返し終了！");
    }

    // 親階層からPlayerタグを探す
    Transform FindPlayerRoot(Transform current)
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
