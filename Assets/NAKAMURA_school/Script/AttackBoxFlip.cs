using UnityEngine;

public class AttackColliderFlip : MonoBehaviour
{
    [Header("向きを参照するオブジェクト")]
    public Transform target;

    [Header("反転するCollider")]
    public Collider2D attackCollider;

    [Header("右向き位置")]
    public Vector2 rightOffset = new Vector2(1f, 0f);

    [Header("左向き位置")]
    public Vector2 leftOffset = new Vector2(-1f, 0f);

    void Awake()
    {
        if (attackCollider == null)
            attackCollider = GetComponent<Collider2D>();

        if (target == null)
            target = transform.root;
    }

    void LateUpdate()
    {
        if (target == null || attackCollider == null)
            return;

        bool facingLeft = target.localScale.x < 0;

        Vector2 offset = facingLeft ? leftOffset : rightOffset;

        switch (attackCollider)
        {
            case BoxCollider2D box:
                box.offset = offset;
                break;

            case CircleCollider2D circle:
                circle.offset = offset;
                break;

            case CapsuleCollider2D capsule:
                capsule.offset = offset;
                break;

            case PolygonCollider2D polygon:
                polygon.offset = offset;
                break;
        }
    }
}