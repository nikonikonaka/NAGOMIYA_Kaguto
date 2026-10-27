using UnityEngine;
using UnityEngine.InputSystem;

public class P_GunConFire : MonoBehaviour
{
    [Header("発射画像Prefab")]
    public GameObject bulletPrefab;

    [Header("発射位置")]
    public Transform shotPoint;

    [Header("プレイヤー")]
    public Player2_con player;

    private bool canShoot = true;

    void Update()
    {
        if (player == null || player.myPad == null)
            return;

        // 向き更新
        UpdateDirection();

        // Xボタンで発射
        if (player.myPad.buttonWest.wasPressedThisFrame &&
            canShoot)
        {
            Shoot();
        }
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        bool flip = player.IsFacingLeft();

        if (shotPoint != null)
        {
            Vector3 pos = shotPoint.localPosition;

            pos.x = flip
                ? -Mathf.Abs(pos.x)
                : Mathf.Abs(pos.x);

            shotPoint.localPosition = pos;
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null)
            return;

        if (shotPoint == null)
            return;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                shotPoint.position,
                Quaternion.identity
            );

        float dir =
            player != null && player.IsFacingLeft()
            ? -1f
            : 1f;

        Projectile projectile =
            bullet.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(
                dir,
                player.gameObject
            );
        }
        SpriteRenderer sr =
            bullet.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = dir < 0;
        }
    }
}