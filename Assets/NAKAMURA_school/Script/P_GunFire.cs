using UnityEngine;

public class P_GunConFire : MonoBehaviour
{
    [Header("発射画像Prefab")]
    public GameObject bulletPrefab;

    [Header("発射位置")]
    public Transform shotPoint;

    [Header("プレイヤー")]
    public Player2_con player;

    public void Shoot()
    {
        if (bulletPrefab == null)
            return;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                shotPoint.position,
                Quaternion.identity
            );

        float dir = player.IsFacingLeft() ? -1f : 1f;

        Projectile p =
            bullet.GetComponent<Projectile>();

        if (p != null)
        {
            p.Initialize(dir);
        }
    }
}