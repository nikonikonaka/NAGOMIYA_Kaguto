
using UnityEngine;

public class AttackBoxFlip : MonoBehaviour
{
    [Header("攻撃するプレイヤー")]
    public Player2_con player;

    [Header("反転する当たり判定")]
    public BoxCollider2D attackBox;

    [Header("右向きの当たり判定位置")]
    public Vector2 rightOffset = new Vector2(1f, 0f);

    [Header("左向きの当たり判定位置")]
    public Vector2 leftOffset = new Vector2(-1f, 0f);

    void Awake()
    {
        if (attackBox == null)
            attackBox = GetComponent<BoxCollider2D>();
    }

    void LateUpdate()
    {
        if (player == null || attackBox == null)
            return;

        // P_MicroConsentと同じ向き判定
        bool facingLeft = player.IsFacingLeft();

        // 当たり判定の位置だけを変更
        if (facingLeft)
        {
            attackBox.offset = leftOffset;
        }
        else
        {
            attackBox.offset = rightOffset;
        }
    }
}
