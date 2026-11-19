using UnityEngine;
using UnityEngine.InputSystem;

public class P_Guncool : MonoBehaviour
{
    [Header("弾")]
    public GameObject bulletPrefab;

    [Header("発射口")]
    public Transform shotPoint;

    [Header("発射設定")]
    public float coolTime = 0.5f; // インスペクターで変更

    private float coolTimer = 0f;

    private Player1_con p1;
    private Player2_con p2;

    private SpriteRenderer sr;

    void Awake()
    {
        p1 = GetComponent<Player1_con>();
        p2 = GetComponent<Player2_con>();

        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (coolTimer > 0)
        {
            coolTimer -= Time.deltaTime;
        }

        Gamepad pad = null;

        if (p1 != null)
        {
            pad = p1.myPad;
        }
        else if (p2 != null)
        {
            pad = p2.myPad;
        }

        if (pad == null)
            return;

        if (pad.buttonWest.wasPressedThisFrame &&
            coolTimer <= 0f)
        {
            Shoot();
            coolTimer = coolTime;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null)
            return;

        if (shotPoint == null)
        {
            Debug.LogWarning("shotPoint が未設定");
            return;
        }

        bool faceLeft = false;

        if (sr != null)
        {
            faceLeft = sr.flipX;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            shotPoint.position,
            Quaternion.identity
        );

        float dir =
            faceLeft ? -1f : 1f;

        Projectile proj =
            bullet.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Initialize(
                dir,
                gameObject
            );
        }

        SpriteRenderer bulletSr =
            bullet.GetComponent<SpriteRenderer>();

        if (bulletSr != null)
        {
            bulletSr.flipX = !faceLeft;
        }

        Debug.Log(
            "Shoot " +
            (faceLeft ? "LEFT" : "RIGHT")
        );
    }
}