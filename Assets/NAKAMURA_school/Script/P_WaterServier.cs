using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class P_WaterServer : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("水量画像")]
    public GameObject waterLv4;
    public GameObject waterLv3;
    public GameObject waterLv2;
    public GameObject waterLv1;
    public GameObject waterEmpty;

    [Header("水攻撃画像")]
    public GameObject waterAnim;

    [Header("水量")]
    public int maxWater = 100;
    public int waterCost = 10;

    [Header("攻撃時間")]
    public float attackTime = 0.4f;

    [Header("水画像位置")]
    public Vector2 waterOffset =
        new Vector2(1.5f, 0f);

    [Header("攻撃範囲")]
    public Vector2 attackOffset =
        new Vector2(1.5f, 0f);

    public Vector2 attackSize =
        new Vector2(3f, 1.5f);

    [Header("ダメージ")]
    public int minDamage = 20;
    public int maxDamage = 80;

    [Header("吹っ飛ばし")]
    public float minForce = 10f;
    public float maxForce = 35f;
    public float attackUpForce = 5f;

    private int currentWater;

    private bool attacking;
    private float attackTimer;
    private float attackDirection;

    private HashSet<Player_Health> hitPlayers =
        new HashSet<Player_Health>();

    void Start()
    {
        currentWater = maxWater;

        UpdateTankImage();

        if (waterAnim != null)
        {
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
            StartWaterAttack();
        }

        if (attacking)
        {
            attackTimer -= Time.deltaTime;

            CheckAttackHit();

            if (attackTimer <= 0f)
            {
                EndAttack();
            }
        }
    }

    void StartWaterAttack()
    {
        if (currentWater <= 0)
            return;

        attacking = true;

        attackTimer = attackTime;

        hitPlayers.Clear();

        bool left =
            player.IsFacingLeft();

        attackDirection =
            left ? -1f : 1f;

        currentWater -= waterCost;

        if (currentWater < 0)
        {
            currentWater = 0;
        }

        HideAllTankImages();

        if (waterAnim != null)
        {
            Vector2 pos =
                waterAnim.transform.localPosition;

            pos.x =
              left
              ? Mathf.Abs(waterOffset.x)
              : -Mathf.Abs(waterOffset.x);

            pos.y = waterOffset.y;

            waterAnim.transform.localPosition =
                pos;

            SetFlip(
                waterAnim,
                left);

            waterAnim.SetActive(true);
        }
    }

    void EndAttack()
    {
        attacking = false;

        if (waterAnim != null)
        {
            waterAnim.SetActive(false);
        }

        UpdateTankImage();
    }

    void CheckAttackHit()
    {
        Vector2 center =
            (Vector2)transform.position +
            new Vector2(
                attackOffset.x * attackDirection,
                attackOffset.y);

        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                center,
                attackSize,
                0f);

        foreach (Collider2D hit in hits)
        {
            Player_Health hp =
                hit.GetComponentInParent<Player_Health>();

            if (hp == null)
                continue;

            if (hp.transform.root ==
                player.transform.root)
                continue;

            if (hitPlayers.Contains(hp))
                continue;

            hitPlayers.Add(hp);

            float ratio =
                (float)currentWater /
                maxWater;

            int damage =
                Mathf.RoundToInt(
                    Mathf.Lerp(
                        minDamage,
                        maxDamage,
                        ratio));

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
                float force =
                    Mathf.Lerp(
                        minForce,
                        maxForce,
                        ratio);

                rb.linearVelocity =
                    new Vector2(
                        attackDirection * force,
                        attackUpForce);
            }
        }
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        bool flip =
            player.IsFacingLeft();

        SetFlip(waterLv4, flip);
        SetFlip(waterLv3, flip);
        SetFlip(waterLv2, flip);
        SetFlip(waterLv1, flip);
        SetFlip(waterEmpty, flip);
    }

    void SetFlip(
        GameObject obj,
        bool flip)
    {
        if (obj == null)
            return;

        Vector3 scale =
            obj.transform.localScale;

        scale.x =
            flip
            ? -Mathf.Abs(scale.x)
            : Mathf.Abs(scale.x);

        obj.transform.localScale =
            scale;
    }

    void HideAllTankImages()
    {
        if (waterLv4 != null)
            waterLv4.SetActive(false);

        if (waterLv3 != null)
            waterLv3.SetActive(false);

        if (waterLv2 != null)
            waterLv2.SetActive(false);

        if (waterLv1 != null)
            waterLv1.SetActive(false);

        if (waterEmpty != null)
            waterEmpty.SetActive(false);
    }

    void UpdateTankImage()
    {
        HideAllTankImages();

        float ratio =
            (float)currentWater /
            maxWater;

        if (ratio > 0.75f)
        {
            waterLv4.SetActive(true);
        }
        else if (ratio > 0.5f)
        {
            waterLv3.SetActive(true);
        }
        else if (ratio > 0.25f)
        {
            waterLv2.SetActive(true);
        }
        else if (ratio > 0f)
        {
            waterLv1.SetActive(true);
        }
        else
        {
            waterEmpty.SetActive(true);
        }
    }

    public void RefillWater()
    {
        currentWater = maxWater;

        UpdateTankImage();
    }

    void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        float dir =
            player.IsFacingLeft()
            ? -1f
            : 1f;

        Vector2 center =
            (Vector2)transform.position +
            new Vector2(
                attackOffset.x * dir,
                attackOffset.y);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(
            center,
            attackSize);
    }
}