using UnityEngine;
using UnityEngine.InputSystem;

public class P_GunConFire : MonoBehaviour
{
    [Header("íe")]
    public GameObject bulletPrefab;

    [Header("î≠éÀå˚")]
    public Transform shotPoint;

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

        if (pad.buttonWest.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null)
            return;

        if (shotPoint == null)
        {
            Debug.LogWarning("shotPoint Ç™ñ¢ê›íË");
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