using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;
    public float lifeTime = 3f;

    private float dir = 1f;

    public void Initialize(float direction)
    {
        dir = direction;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position +=
            Vector3.right * dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player_Health hp =
                col.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}