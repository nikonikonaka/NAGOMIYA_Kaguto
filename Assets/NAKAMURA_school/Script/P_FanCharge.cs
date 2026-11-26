using UnityEngine;
using UnityEngine.InputSystem;

public class P_FanCharge : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("地面判定")]
    public Collider2D playerCollider;
    public LayerMask groundLayers;
    public float groundCheckDepth = 0.08f;

    [Header("オーラ")]
    public GameObject auraObject;

    [Header("最大チャージ")]
    public int maxAura = 10;

    [Header("オーラが0になった後のチャージ禁止時間")]
    public float cooldownDuration = 5f;

    private float auraCount = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (playerCollider == null)
            playerCollider = GetComponent<Collider2D>();

        // 初回のブースト用に開始時はオーラ満タン
        auraCount = Mathf.Max(0, maxAura);
        UpdateAura();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

        if (player == null || player.myPad == null)
            return;

        if (player.myPad.buttonWest.wasPressedThisFrame && CanCharge())
            Charge();
    }

    bool IsGrounded()
    {
        if (playerCollider == null)
            return false;

        Bounds bounds = playerCollider.bounds;
        float depth = Mathf.Max(0.01f, groundCheckDepth);

        // Ground Layersが未設定なら、プレイヤー自身のレイヤー以外を調べる
        int mask = groundLayers.value != 0
            ? groundLayers.value
            : ~(1 << playerCollider.gameObject.layer);

        Vector2 probeSize = new Vector2(
            Mathf.Max(0.05f, bounds.size.x * 0.75f),
            depth
        );

        Vector2 probePosition = new Vector2(
            bounds.center.x,
            bounds.min.y - depth * 0.5f + 0.01f
        );

        return Physics2D.OverlapBox(probePosition, probeSize, 0f, mask) != null;
    }

    void Charge()
    {
        if (!CanCharge())
            return;

        auraCount = Mathf.Min(auraCount + 0.25f, maxAura);
        UpdateAura();
    }

    public bool CanCharge()
    {
        return IsGrounded() && cooldownTimer <= 0f && auraCount < maxAura;
    }

    public bool HasAura()
    {
        return auraCount > 0f;
    }

    public int GetAura()
    {
        return (int)auraCount;
    }

    public void ConsumeAura()
    {
        if (auraCount <= 0f)
            return;

        auraCount = Mathf.Max(0f, auraCount - 0.5f);

        if (auraCount <= 0f)
            cooldownTimer = cooldownDuration;

        UpdateAura();
    }

    public void ResetAura()
    {
        auraCount = 0f;
        cooldownTimer = 0f;
        UpdateAura();
    }

    void UpdateAura()
    {
        if (auraObject == null)
            return;

        SpriteRenderer sr = auraObject.GetComponent<SpriteRenderer>();
        if (sr == null)
            return;

        float alpha = maxAura > 0 ? auraCount / maxAura : 0f;

        Color color = sr.color;
        color.a = alpha;
        sr.color = color;

        auraObject.transform.localScale = Vector3.one * (1f + alpha);
    }
}


/*using UnityEngine;
using UnityEngine.InputSystem;

public class P_FanCharge : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("GroundCheck")]
    public GroundCheck groundCheck;

    [Header("オーラ")]
    public GameObject auraObject;

    [Header("最大チャージ")]
    public int maxAura = 10;

    private float auraCount = 0f;

    private bool auraCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        UpdateAura();
    }

    void Update()
    {
        if (player == null)
            return;

        if (player.myPad == null)
            return;

        // オーラ再使用クールタイム
        if (auraCooldown)
        {
            if (groundCheck != null &&
                groundCheck.isGround)
            {
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer >= 3.0f)
                {
                    auraCooldown = false;
                    cooldownTimer = 0f;
                }
            }
        }

        // 地面についている時だけチャージ
        if (groundCheck != null &&
            groundCheck.isGround &&
            player.myPad.buttonWest.wasPressedThisFrame)
        {
            Charge();
        }
    }

    void Charge()
    {
        if (auraCount >= maxAura)
            return;

        auraCount += 0.25f;

        UpdateAura();
    }

    public bool HasAura()
    {
        return auraCount > 0;
    }

    public int GetAura()
    {
        return (int)auraCount;
    }

    public void ConsumeAura()
    {
        if (auraCooldown)
            return;

        if (auraCount <= 0)
            return;

        auraCount -= 0.5f;

        if (auraCount < 0)
            auraCount = 0;

        auraCooldown = true;
        cooldownTimer = 0f;

        UpdateAura();
    }

    public void ResetAura()
    {
        auraCount = 0f;

        auraCooldown = false;
        cooldownTimer = 0f;

        UpdateAura();
    }

    public bool CanUseAura()
    {
        return !auraCooldown;
    }

    void UpdateAura()
    {
        if (auraObject == null)
            return;

        SpriteRenderer sr =
            auraObject.GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        float alpha =
            auraCount / maxAura;

        Color c = sr.color;

        c.a = alpha;

        sr.color = c;

        auraObject.transform.localScale =
            Vector3.one * (1.0f + alpha);
    }
}
*/

