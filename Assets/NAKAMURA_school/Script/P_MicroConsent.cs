
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class P_MicroConsent : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player2_con player;

    [Header("コンセントアニメ")]
    public GameObject consentAnim;

    [Header("Animator")]
    public Animator consentAnimator;

    [Header("攻撃")]
    public float attackRadius = 2f;
    public int damage = 20;

    [Header("吹っ飛ばし")]
    public float attackForce = 15f;
    public float launchY = 5f;
    public float knockbackTime = 0.3f;

    private float lastPressTime = -1f;
    private float doubleClickTime = 0.3f;

    private bool attacking = false;

    void Start()
    {
        if (consentAnim != null)
        {
            consentAnim.SetActive(false);
        }
    }

    void Update()
    {
        UpdateDirection();

        if (player == null || player.myPad == null)
            return;

        if (player.myPad.buttonWest.wasPressedThisFrame)
        {
            if (Time.time - lastPressTime <= doubleClickTime)
            {
                StartAttack();
            }

            lastPressTime = Time.time;
        }
    }

    // 攻撃アニメーションの向き
    void UpdateDirection()
    {
        if (player == null || consentAnim == null)
            return;

        bool flip = player.IsFacingLeft();

        SpriteRenderer[] renderers =
            consentAnim.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            sr.flipX = flip;
        }
    }

    // 攻撃開始
    void StartAttack()
    {
        if (attacking)
            return;

        attacking = true;

        if (consentAnim != null)
        {
            consentAnim.SetActive(true);
        }

        if (consentAnimator != null)
        {
            consentAnimator.Rebind();
            consentAnimator.Update(0f);
            consentAnimator.Play("Consent", 0, 0f);
        }

        Invoke(nameof(HitAttack), 0.2f);
        Invoke(nameof(EndAttack), 0.8f);
    }

    // 攻撃判定
    void HitAttack()
    {
        if (player == null)
            return;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                attackRadius
            );

        // 同じ相手に複数回当たることを防ぐ
        HashSet<Rigidbody2D> attackedTargets =
            new HashSet<Rigidbody2D>();

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            // 攻撃した本人を除外
            if (hit.transform.root == player.transform.root)
                continue;

            // 相手のRigidbody2D
            Rigidbody2D targetRb =
                hit.GetComponentInParent<Rigidbody2D>();

            if (targetRb == null)
                continue;

            if (attackedTargets.Contains(targetRb))
                continue;

            attackedTargets.Add(targetRb);

            // Dynamic以外は吹っ飛ばせない
            if (targetRb.bodyType != RigidbodyType2D.Dynamic)
            {
                Debug.LogWarning(
                    "相手のRigidbody2DをDynamicにしてください: "
                    + targetRb.gameObject.name
                );

                continue;
            }

            // 相手の体力
            Player_Health hp =
                hit.GetComponentInParent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            // 相手のプレイヤー取得
            Player1_con p1 =
                targetRb.GetComponentInParent<Player1_con>();

            Player2_con p2 =
                targetRb.GetComponentInParent<Player2_con>();

            // 相手の左右位置を判定
            float dir =
                targetRb.position.x >= player.transform.position.x
                ? 1f
                : -1f;

            // 操作ロック
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

            Debug.Log(
                "吹っ飛ばし開始: " +
                targetRb.gameObject.name +
                " / 横方向=" + dir
            );

            // 吹っ飛ばし開始
            StartCoroutine(
                KnockbackPlayer(
                    targetRb,
                    dir,
                    p1,
                    p2
                )
            );
        }

        Debug.Log("コンセント攻撃！");
    }

    // 吹っ飛ばし処理
    IEnumerator KnockbackPlayer(
        Rigidbody2D targetRb,
        float dir,
        Player1_con p1,
        Player2_con p2)
    {
        if (targetRb == null)
            yield break;

        float timer = 0f;

        // 最初に横方向と上方向へ飛ばす
        targetRb.linearVelocity = new Vector2(
            dir * attackForce,
            launchY
        );

        while (timer < knockbackTime)
        {
            if (targetRb == null)
                yield break;

            // 横方向の速度を維持
            // 上方向は重力による変化を許可
            targetRb.linearVelocity = new Vector2(
                dir * attackForce,
                targetRb.linearVelocity.y
            );

            timer += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        // 操作ロック解除
        if (p1 != null)
        {
            p1.lockControl = false;
            p1.knockback = false;
        }

        if (p2 != null)
        {
            p2.lockControl = false;
            p2.knockback = false;
        }

        Debug.Log("吹っ飛ばし終了！");
    }

    // 攻撃終了
    void EndAttack()
    {
        if (consentAnim != null)
        {
            consentAnim.SetActive(false);
        }

        attacking = false;
    }

    // 攻撃範囲の表示
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRadius
        );
    }
}
