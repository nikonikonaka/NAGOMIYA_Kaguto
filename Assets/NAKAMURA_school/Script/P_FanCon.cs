using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class P_FanServer : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("風チャージ")]
    public P_FanCharge fanCharge;

    [Header("風エフェクト")]
    public GameObject waterAnim;

    [Header("攻撃範囲")]
    public Vector2 attackOffset =
        new Vector2(1.5f, 0f);

    public Vector2 attackSize =
        new Vector2(3f, 1.5f);

    [Header("ダメージ")]
    public int damage = 10;

    [Header("吹っ飛ばし")]
    public float force = 35f;

    public float attackUpForce = 3f;

    [Header("オーラ消費間隔")]
    public float consumeInterval = 0.2f;

    private bool attacking;

    private float attackDirection;

    private float consumeTimer;

    private Vector3 defaultLocalPos;

    private HashSet<Player_Health> hitPlayers =
        new HashSet<Player_Health>();

    void Start()
    {
        if (waterAnim != null)
        {
            defaultLocalPos =
                waterAnim.transform.localPosition;

            waterAnim.SetActive(false);
        }
    }

    void Update()
    {
        UpdateDirection();

        if (player == null)
            return;

        if (player.myPad == null)
            return;

        if (!attacking &&
            player.myPad.buttonEast.wasPressedThisFrame)
        {
            if (fanCharge != null &&
                fanCharge.HasAura())
            {
                StartFanAttack();
            }
        }

        if (attacking)
        {
            CheckAttackHit();

            consumeTimer += Time.deltaTime;

            if (fanCharge != null &&
                consumeTimer >= consumeInterval)
            {
                consumeTimer = 0f;

                    fanCharge.ConsumeAura();

                 
                if (!fanCharge.HasAura())
                {
                    EndAttack();
                }
            }
        }
    }

    void StartFanAttack()
    {
        consumeTimer = 0f;

        attacking = true;

        hitPlayers.Clear();

        bool left =
            player.IsFacingLeft();

        attackDirection =
            left ? -1f : 1f;

        if (waterAnim != null)
        {
            Vector3 pos =
                defaultLocalPos;

            pos.x = left
                ? -Mathf.Abs(defaultLocalPos.x)
                : Mathf.Abs(defaultLocalPos.x);

            waterAnim.transform.localPosition =
                pos;

            waterAnim.SetActive(true);
        }
    }

    void EndAttack()
    {
        attacking = false;

        consumeTimer = 0f;

        if (waterAnim != null)
        {
            waterAnim.SetActive(false);
        }
    }

    void CheckAttackHit()
    {
        bool left =
            player.IsFacingLeft();

        Vector2 center =
            (Vector2)transform.position +
            new Vector2(
                left
                    ? -Mathf.Abs(attackOffset.x)
                    : Mathf.Abs(attackOffset.x),
                attackOffset.y
            );

        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                center,
                attackSize,
                0f
            );

        foreach (Collider2D hit in hits)
        {
            Player_Health hp =
                hit.GetComponentInParent<Player_Health>();

            if (hp == null)
                continue;

            Player1_con myP1 =
                GetComponent<Player1_con>();

            Player2_con myP2 =
                GetComponent<Player2_con>();

            Player1_con targetP1 =
                hit.GetComponentInParent<Player1_con>();

            Player2_con targetP2 =
                hit.GetComponentInParent<Player2_con>();

            if (
                (myP1 != null && targetP1 == myP1) ||
                (myP2 != null && targetP2 == myP2)
            )
            {
                continue;
            }

            if (hitPlayers.Contains(hp))
                continue;

            hitPlayers.Add(hp);

            hp.TakeDamage(damage);

            Rigidbody2D rb =
                hp.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                rb =
                    hp.GetComponentInParent<Rigidbody2D>();
            }

            if (rb != null)
            {
                rb.linearVelocity =
                    new Vector2(
                        attackDirection * force,
                        attackUpForce
                    );
            }
        }
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        if (waterAnim == null)
            return;

        bool left =
            player.IsFacingLeft();

        Vector3 pos =
            defaultLocalPos;

        pos.x = left
            ? -Mathf.Abs(defaultLocalPos.x)
            : Mathf.Abs(defaultLocalPos.x);

        waterAnim.transform.localPosition =
            pos;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        bool left =
            player.IsFacingLeft();

        Vector2 center =
            (Vector2)transform.position +
            new Vector2(
                left
                    ? -Mathf.Abs(attackOffset.x)
                    : Mathf.Abs(attackOffset.x),
                attackOffset.y
            );

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(
            center,
            attackSize
        );
    }
}