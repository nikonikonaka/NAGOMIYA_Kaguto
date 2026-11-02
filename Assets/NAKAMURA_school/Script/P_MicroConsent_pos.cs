using UnityEngine;
using UnityEngine.InputSystem;

public class P_MicroConsent_pos : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player2_con player;

    [Header("コンセントアニメ")]
    public GameObject consentAnim;

    [Header("Animator")]
    public Animator consentAnimator;

    [Header("表示位置")]
    public Vector2 attackOffset = new Vector2(1f, 0f);

    [Header("攻撃")]
    public float attackRadius = 2f;
    public int damage = 20;
    public float attackForce = 500f;

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

    void UpdateConsentPosition()
    {
        if (player == null || consentAnim == null)
            return;

        Vector2 offset = attackOffset;

        if (player.IsFacingLeft())
        {
            offset.x = -Mathf.Abs(offset.x);
        }
        else
        {
            offset.x = Mathf.Abs(offset.x);
        }

        consentAnim.transform.position =
            (Vector2)transform.position + offset;
    }

    void StartAttack()
    {
        if (attacking)
            return;

        attacking = true;

        UpdateConsentPosition();

        consentAnim.SetActive(true);

        if (consentAnimator != null)
        {
            consentAnimator.Rebind();
            consentAnimator.Update(0f);
            consentAnimator.Play("Consent", 0, 0f);
        }

        Invoke(nameof(HitAttack), 0.2f);
        Invoke(nameof(EndAttack), 0.8f);
    }

    void HitAttack()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                consentAnim.transform.position,
                attackRadius
            );

        foreach (Collider2D hit in hits)
        {
            if (player != null &&
                hit.gameObject == player.gameObject)
            {
                continue;
            }

            Player_Health hp =
                hit.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Rigidbody2D rb =
                hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 dir =
                    ((Vector2)hit.transform.position -
                    (Vector2)consentAnim.transform.position).normalized;

                rb.AddForce(dir * attackForce);
            }
        }

        Debug.Log("コンセント攻撃！");
    }

    void EndAttack()
    {
        if (consentAnim != null)
        {
            consentAnim.SetActive(false);
        }

        attacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (consentAnim != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                consentAnim.transform.position,
                attackRadius
            );
        }
    }
}