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

    void Awake()
    {
        p1 = GetComponent<Player1_con>();
        p2 = GetComponent<Player2_con>();
    }

    void Update()
    {
        Gamepad pad = null;
        bool faceLeft = false;

        if (p1 != null)
        {
            pad = p1.myPad;
            faceLeft = p1.IsFacingLeft();
        }
        else if (p2 != null)
        {
            pad = p2.myPad;
            faceLeft = p2.IsFacingLeft();
        }

        if (pad == null)
            return;

        if (pad.buttonWest.wasPressedThisFrame)
        {
            Shoot(faceLeft);
        }
    }

    void Shoot(bool faceLeft)
    {
        if (bulletPrefab == null)
            return;

        if (shotPoint == null)
        {
            Debug.LogWarning("shotPoint Ç™ñ¢ê›íË");
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            shotPoint.position,
            Quaternion.identity
        );

        float dir = faceLeft ? -1f : 1f;

        Projectile proj = bullet.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Initialize(
                dir,
                gameObject
            );
        }

        SpriteRenderer sr =
            bullet.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = faceLeft;
        }

        Debug.Log(
            $"Shoot: {(faceLeft ? "LEFT" : "RIGHT")} " +
            $"Pos={shotPoint.position}"
        );
    }
}