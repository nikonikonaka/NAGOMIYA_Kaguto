using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("弾設定")]
    public float speed = 10f;
    public int damage = 20;
    public float lifeTime = 3f;

    [Header("発射クールタイム")]
    public float coolTime = 1.0f;   // インスペクターで変更可能

    private float dir = 1f;
    private GameObject owner;

    public void Initialize(float direction, GameObject shooter)
    {
        dir = direction;
        owner = shooter;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position +=
            Vector3.right * dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 発射した本人には当たらない
        if (col.gameObject == owner)
            return;

        if (col.CompareTag("Player"))
        {
            Player_Health hp = col.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}