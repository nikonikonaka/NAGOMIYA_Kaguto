using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Microwave_con : MonoBehaviour
{
    [Header("画像オブジェクト")]
    public GameObject defaultImage;
    public GameObject eggImage;
    public GameObject heatingImage;
    public GameObject explosionImage;
    public GameObject openedImage;

    [Header("プレイヤー")]
    public Player2_con player;

    [Header("爆発")]
    public float heatTime = 3f;
    public float explosionRadius = 3f;
    public int damage = 30;
    public float explosionForce = 800f;

    private bool heating = false;

    void Start()
    {
        ShowImage(defaultImage);
    }

    void Update()
    {
        UpdateDirection();

        if (Gamepad.current == null)
            return;

        // Yボタン
        if (Gamepad.current.buttonNorth.wasPressedThisFrame &&
            !heating)
        {
            StartCoroutine(HeatRoutine());
        }
    }

    void UpdateDirection()
    {
        if (player == null)
            return;

        bool flip = player.IsFacingLeft();

        SetFlip(defaultImage, flip);
        SetFlip(eggImage, flip);
        SetFlip(heatingImage, flip);
        SetFlip(explosionImage, flip);
        SetFlip(openedImage, flip);
    }

    void SetFlip(GameObject obj, bool flip)
    {
        if (obj == null)
            return;

        SpriteRenderer sr =
            obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipX = flip;
        }
    }

    IEnumerator HeatRoutine()
    {
        heating = true;

        if (player != null)
        {
            player.lockControl = true;
        }

        // 卵投入
        ShowImage(eggImage);

        yield return new WaitForSeconds(0.5f);

        // 加熱中
        ShowImage(heatingImage);

        yield return new WaitForSeconds(heatTime);

        // 爆発
        ShowImage(explosionImage);

        Explode();

        yield return new WaitForSeconds(1f);

        // 開いた状態
        ShowImage(openedImage);

        yield return new WaitForSeconds(1f);

        // 閉じる
        ShowImage(defaultImage);

        if (player != null)
        {
            player.lockControl = false;
        }

        heating = false;
    }

    void ShowImage(GameObject target)
    {
        SetVisible(defaultImage, false);
        SetVisible(eggImage, false);
        SetVisible(heatingImage, false);
        SetVisible(explosionImage, false);
        SetVisible(openedImage, false);

        SetVisible(target, true);
    }

    void SetVisible(GameObject obj, bool visible)
    {
        if (obj == null)
            return;

        SpriteRenderer spriteRenderer =
            obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }

    void Explode()
    {
        Collider2D[] targets =
            Physics2D.OverlapCircleAll(
                transform.position,
                explosionRadius
            );

        foreach (Collider2D target in targets)
        {
            Player_Health hp =
                target.GetComponent<Player_Health>();

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Rigidbody2D rb =
                target.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 dir =
                    (target.transform.position -
                     transform.position).normalized;

                rb.AddForce(dir * explosionForce);
            }
        }

        Debug.Log("レンジ爆発！");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}