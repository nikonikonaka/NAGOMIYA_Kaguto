using UnityEngine;

public class CharacterSelectPreview : MonoBehaviour
{
    [Header("キャラクターPrefab")]
    public GameObject[] characterPrefabs;

    [Header("表示位置")]
    public Transform player1PreviewPoint;
    public Transform player2PreviewPoint;

    private GameObject player1Preview;
    private GameObject player2Preview;

    private int player1Index = 0;
    private int player2Index = 1;

    void Start()
    {
        ShowPlayer1();
        ShowPlayer2();
    }

    void Update()
    {
        // P1：Aキーで左、Dキーで右
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePlayer1(-1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePlayer1(1);
        }

        // P2：左矢印で左、右矢印で右
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePlayer2(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePlayer2(1);
        }
    }

    void ChangePlayer1(int direction)
    {
        player1Index += direction;

        if (player1Index < 0)
        {
            player1Index = characterPrefabs.Length - 1;
        }

        if (player1Index >= characterPrefabs.Length)
        {
            player1Index = 0;
        }

        ShowPlayer1();
    }

    void ChangePlayer2(int direction)
    {
        player2Index += direction;

        if (player2Index < 0)
        {
            player2Index = characterPrefabs.Length - 1;
        }

        if (player2Index >= characterPrefabs.Length)
        {
            player2Index = 0;
        }

        ShowPlayer2();
    }

    void ShowPlayer1()
    {
        if (player1Preview != null)
        {
            Destroy(player1Preview);
        }

        player1Preview = Instantiate(
            characterPrefabs[player1Index],
            player1PreviewPoint.position,
            Quaternion.identity
        );

        SetupPreview(player1Preview);
    }

    void ShowPlayer2()
    {
        if (player2Preview != null)
        {
            Destroy(player2Preview);
        }

        player2Preview = Instantiate(
            characterPrefabs[player2Index],
            player2PreviewPoint.position,
            Quaternion.identity
        );

        SetupPreview(player2Preview);
    }

    void SetupPreview(GameObject character)
    {
        // キャラ選択中は戦闘用の操作を停止する
        Player1_con p1 = character.GetComponent<Player1_con>();
        if (p1 != null)
        {
            p1.enabled = false;
        }

        Player2_con p2 = character.GetComponent<Player2_con>();
        if (p2 != null)
        {
            p2.enabled = false;
        }

        // Rigidbody2Dの落下も停止する
        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
