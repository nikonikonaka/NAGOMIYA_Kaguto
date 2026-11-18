using UnityEngine;
using UnityEngine.InputSystem;

public class P_WaterCharge : MonoBehaviour
{
    [Header("プレイヤー")]
    public Player1_con player;

    [Header("ウォーターサーバー")]
    public P_WaterServer waterServer;

    [Header("オーラ")]
    public GameObject auraObject;

    [Header("チャージ")]
    public int maxAura = 10;

    public int waterPerCharge = 10;

    private int auraCount = 0;

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
        auraCount++;

        UpdateAura();

        if (auraCount >= maxAura)
        {
            auraCount = 0;

            if (waterServer != null)
            {
                waterServer.AddWater(
                    waterPerCharge
                );
            }

            UpdateAura();

            Debug.Log(
                "給水成功"
            );
        }
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
            Vector3.one * (1f + alpha);
    }
}