using UnityEngine;

public class WaterPush : MonoBehaviour
{
    [Header("吹っ飛ばし")]
    public float knockbackX = 8f;

    public float knockbackY = 2f;

    [Header("ダメージ")]
    public int damage = 10;

    [Header("発射したプレイヤー")]
    public GameObject owner;

    public void SetOwner(GameObject shooter)
    {
        owner = shooter;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player_Health hp =
            other.GetComponentInParent<Player_Health>();

        if (hp == null)
            return;

        // 発射した本人は無視
        if (
            owner != null &&
            hp.transform.root ==
            owner.transform.root
        )
        {
            return;
        }

        Rigidbody2D rb =
            hp.GetComponentInParent<Rigidbody2D>();

        if (rb == null)
            return;

        float dir =
            rb.position.x >= transform.position.x
            ? 1f
            : -1f;

        hp.TakeDamage(damage);

        // 水っぽく押し流す
        rb.linearVelocity =
            new Vector2(
                dir * knockbackX,
                knockbackY
            );

        Debug.Log(
            "WaterPush : " +
            hp.gameObject.name
        );
    }
}