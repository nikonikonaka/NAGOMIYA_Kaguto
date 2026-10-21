using UnityEngine;

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

    void StartAttack()
    {
        attacking = true;
        timer = attackTime;

        ShowImage(spinImage);

        // 毎回最初から再生
        Animator anim = spinImage.GetComponent<Animator>();

        if (anim != null)
        {
            anim.Play(0, 0, 0f);
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
}