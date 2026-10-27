using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectPreview : MonoBehaviour
{
    [Header("コントローラー管理")]
    public Joycon joycon;

    [Header("キャラクターPrefab")]
    public GameObject[] characterPrefabs;

    [Header("表示位置")]
    public Transform player1PreviewPoint;
    public Transform player2PreviewPoint;

    private GameObject player1Preview;
    private GameObject player2Preview;

    private int player1Index = 0;
    private int player2Index = 1;

    // スティックを倒し続けても1回だけ変更するための変数
    private bool p1LeftHeld;
    private bool p1RightHeld;
    private bool p2LeftHeld;
    private bool p2RightHeld;

    void Start()
    {
        if (characterPrefabs == null ||
            characterPrefabs.Length == 0)
        {
            Debug.LogError("キャラクターPrefabが登録されていません！");
            return;
        }

        player1Index = 0;
        player2Index = characterPrefabs.Length > 1 ? 1 : 0;

        ShowPlayer1();
        ShowPlayer2();
    }

    void Update()
    {
        if (joycon == null)
            return;

        // 1Pと2Pのコントローラーを個別に取得
        Gamepad p1 = joycon.Player1Pad;
        Gamepad p2 = joycon.Player2Pad;

        // 1Pの操作
        if (p1 != null)
        {
            CheckPlayer1(p1);
        }

        // 2Pの操作
        if (p2 != null)
        {
            CheckPlayer2(p2);
        }
    }

    void CheckPlayer1(Gamepad pad)
    {
        bool left =
            pad.dpad.left.isPressed ||
            pad.leftStick.x.ReadValue() < -0.5f;

        bool right =
            pad.dpad.right.isPressed ||
            pad.leftStick.x.ReadValue() > 0.5f;

        if (left && !p1LeftHeld)
        {
            ChangePlayer1(-1);
        }
        else if (right && !p1RightHeld)
        {
            ChangePlayer1(1);
        }

        p1LeftHeld = left;
        p1RightHeld = right;
    }

    void CheckPlayer2(Gamepad pad)
    {
        bool left =
            pad.dpad.left.isPressed ||
            pad.leftStick.x.ReadValue() < -0.5f;

        bool right =
            pad.dpad.right.isPressed ||
            pad.leftStick.x.ReadValue() > 0.5f;

        if (left && !p2LeftHeld)
        {
            ChangePlayer2(-1);
        }
        else if (right && !p2RightHeld)
        {
            ChangePlayer2(1);
        }

        p2LeftHeld = left;
        p2RightHeld = right;
    }

    void ChangePlayer1(int direction)
    {
        if (characterPrefabs.Length == 0)
            return;

        player1Index =
            (player1Index + direction + characterPrefabs.Length)
            % characterPrefabs.Length;

        ShowPlayer1();
    }

    void ChangePlayer2(int direction)
    {
        if (characterPrefabs.Length == 0)
            return;

        player2Index =
            (player2Index + direction + characterPrefabs.Length)
            % characterPrefabs.Length;

        ShowPlayer2();
    }

    void ShowPlayer1()
    {
        if (player1Preview != null)
            Destroy(player1Preview);

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
            Destroy(player2Preview);

        player2Preview = Instantiate(
            characterPrefabs[player2Index],
            player2PreviewPoint.position,
            Quaternion.identity
        );

        SetupPreview(player2Preview);
    }

    void SetupPreview(GameObject character)
    {
        // キャラ選択中は戦闘操作を停止
        Player1_con p1 =
            character.GetComponent<Player1_con>();

        if (p1 != null)
            p1.enabled = false;

        Player2_con p2 =
            character.GetComponent<Player2_con>();

        if (p2 != null)
            p2.enabled = false;

        // 落下や物理移動を停止
        Rigidbody2D rb =
            character.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
