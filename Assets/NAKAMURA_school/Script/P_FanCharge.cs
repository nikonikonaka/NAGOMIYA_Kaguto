using UnityEngine;
using UnityEngine.InputSystem;

public class P_FanCharge : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("オーラ")]
    public GameObject auraObject;

    [Header("最大チャージ")]
    public int maxAura = 10;

    private float auraCount = 0;

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

        if (player.myPad.buttonWest.wasPressedThisFrame)
        {
            Charge();
        }
    }

    void Charge()
    {
        if (auraCount >= maxAura)
            return;

        auraCount+= (float)0.25;


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
        if (auraCount <= 0)
            return;

        auraCount--;

        UpdateAura();
    }

    public void ResetAura()
    {
        auraCount = 0;

        UpdateAura();
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
            (float)auraCount / maxAura;

        Color c = sr.color;

        c.a = alpha;

        sr.color = c;

        auraObject.transform.localScale =
            Vector3.one * (1.0f + alpha);
    }
}