using UnityEngine;
using UnityEngine.InputSystem;

public class P_Springcon : MonoBehaviour
{
    [Header("画像")]
    public GameObject defaultImage;
    public GameObject spinImage;

    [Header("オーラ")]
    public GameObject auraObject;

    [Header("プレイヤー")]
    public Player1_con player;

    [Header("突進")]
    public float attackTime = 1f;
    public int damage = 20;

    [Header("速度")]
    public float baseSpeed = 15f;
    public float auraSpeed = 8f;

    [Header("オーラ設定")]
    public int maxAura = 10;
    public float auraResetTime = 3f;

    private int auraCount = 0;
    private float auraTimer = 0f;

    private bool attacking = false;
    private float timer;

    private Rigidbody2D rb;

    private float attackDirection;
    private float attackVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ShowImage(defaultImage);

        if (auraObject != null)
        {
            SetAuraAlpha(0f);
        }
    }

    void Update()
    {
        UpdateDirection();

        if (player == null || player.myPad == null)
            return;

        // Xでオーラ
        if (player.myPad.buttonWest.wasPressedThisFrame)
        {
            auraCount++;

            if (auraCount > maxAura)
                auraCount = maxAura;

            auraTimer = auraResetTime;

            SetAuraAlpha(
                (float)auraCount / maxAura
            );
        }

        // Bで突進
        if (!attacking &&
            player.myPad.buttonEast.wasPressedThisFrame)
        {
            StartAttack();
        }

        // オーラ消滅
        if (auraCount > 0)
        {
            auraTimer -= Time.deltaTime;

            if (auraTimer <= 0f)
            {
                auraCount = 0;
                SetAuraAlpha(0f);
            }
        }

        // 攻撃終了
        if (attacking)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                attacking = false;

                rb.linearVelocity =
                    new Vector2(
                        0f,
                        rb.linearVelocity.y
                    );

                ShowImage(defaultImage);

                auraCount = 0;

                SetAuraAlpha(0f);
            }
        }
    }

    void FixedUpdate()
    {
        if (!attacking)
            return;

        rb.linearVelocity =
            new Vector2(
                attackDirection * attackVelocity,
                rb.linearVelocity.y
            );
    }

    public void StartAttack()
    {
        attacking = true;
        timer = attackTime;

        ShowImage(spinImage);

        Animator anim =
            spinImage.GetComponent<Animator>();

        if (anim != null)
        {
            anim.Play("Spring", 0, 0f);
            anim.Update(0f);
        }

        attackDirection =
            player.IsFacingLeft()
            ? -1f
            : 1f;

        attackVelocity =
            baseSpeed +
            auraCount * auraSpeed;
    }

    void SetAuraAlpha(float alpha)
    {
        if (auraObject == null)
            return;

        SpriteRenderer sr =
            auraObject.GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        Color c = sr.color;
        c.a = alpha;
        sr.color = c;

        auraObject.transform.localScale =
            Vector3.one * (1f + alpha);
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        bool flip =
            player.IsFacingLeft();

        SetFlip(defaultImage, flip);
        SetFlip(spinImage, flip);
        SetFlip(auraObject, flip);
    }

    void SetFlip(GameObject obj, bool flip)
    {
        if (obj == null)
            return;

        SpriteRenderer sr =
            obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = flip;
        }
    }

    void ShowImage(GameObject target)
    {
        SetVisible(defaultImage, false);
        SetVisible(spinImage, false);

        SetVisible(target, true);
    }

    void SetVisible(GameObject obj, bool visible)
    {
        if (obj == null)
            return;

        SpriteRenderer sr =
            obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.enabled = visible;
        }
    }

    public void UsestartAttack()
    {
        StartAttack();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!attacking)
            return;

        if (col.gameObject.CompareTag("Player"))
        {
            Player_Health hp =
                col.gameObject.GetComponent<Player_Health>();

            if (hp != null)
            {
                int finalDamage =
                    damage +
                    auraCount * 5;

                hp.TakeDamage(finalDamage);
            }
        }
    }
}

/*using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class P_Springcon : MonoBehaviour
{
    [Header("画像")]
    public GameObject defaultImage;
    public GameObject spinImage;

    [Header("プレイヤー")]
    public Player1_con player;

    [Header("攻撃")]
    public float moveSpeed = 8f;
    public float attackTime = 1f;
    public int damage = 20;

    private bool attacking = false;
    private float timer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ShowImage(defaultImage);
    }

    void Update()
    {
        if (player == null || player.myPad == null)
            return;

        UpdateDirection();

        // B + 左右
        if (!attacking &&
            player.myPad.buttonEast.wasPressedThisFrame &&
            Mathf.Abs(player.myPad.leftStick.x.ReadValue()) > 0.3f)
        {
            StartAttack();
        }

        if (attacking)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                attacking = false;

                ShowImage(defaultImage);
            }
        }
    }

    public void StartAttack()
    {
        attacking = true;
        timer = attackTime;

        ShowImage(spinImage);

        // 毎回最初から再生
        Animator anim = spinImage.GetComponent<Animator>();

        if (anim != null)
        {
            anim.Play("Spring", 0, 0f);
            anim.Update(0f);
        }
    }

    void FixedUpdate()
    {
        if (!attacking)
            return;

        float dir =
            player.myPad.leftStick.x.ReadValue() > 0
            ? 1f
            : -1f;

        rb.linearVelocity =
            new Vector2(
                dir * moveSpeed,
                rb.linearVelocity.y
            );
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        bool flip = player.IsFacingLeft();

        SetFlip(defaultImage, flip);
        SetFlip(spinImage, flip);
    }

    void SetFlip(GameObject obj, bool flip)
    {
        if (obj == null)
            return;

        SpriteRenderer sr =
            obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = flip;
        }
    }

    public void UsestartAttack()
    {
        StartAttack();
    }

    void ShowImage(GameObject target)
    {
        SetVisible(defaultImage, false);
        SetVisible(spinImage, false);

        SetVisible(target, true);
    }

    void SetVisible(GameObject obj, bool visible)
    {
        if (obj == null)
            return;

        SpriteRenderer sr =
            obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.enabled = visible;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!attacking)
            return;

        if (col.gameObject.CompareTag("Player"))
        {
            Player_Health hp =
                col.gameObject.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}*/