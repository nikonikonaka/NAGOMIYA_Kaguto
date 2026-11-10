using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelectPreview : MonoBehaviour
{
    [Header("コントローラー管理")]
    public Joycon joycon;

    [Header("キャラクターPrefab")]
    public GameObject[] characterPrefabs;

    [Header("表示位置")]
    public Transform player1PreviewPoint;
    public Transform player2PreviewPoint;

    [Header("バトルシーン名")]
    public string battleSceneName = "scene_10_15_kansei";

    private GameObject player1Preview;
    private GameObject player2Preview;

    private int player1Index = 0;
    private int player2Index = 1;

    // 左右入力の連続変更防止
    private bool p1LeftHeld;
    private bool p1RightHeld;
    private bool p2LeftHeld;
    private bool p2RightHeld;

    // キャラクター決定状態
    private bool player1Confirmed;
    private bool player2Confirmed;

    // シーン移動の重複防止
    private bool isLoading;

    void Start()
    {
        if (characterPrefabs == null ||
            characterPrefabs.Length == 0)
        {
            Debug.LogError("キャラクターPrefabが登録されていません！");
            return;
        }

        if (player1PreviewPoint == null ||
            player2PreviewPoint == null)
        {
            Debug.LogError("キャラクターの表示位置が設定されていません！");
            return;
        }

        player1Index = 0;
        player2Index = characterPrefabs.Length > 1 ? 1 : 0;

        ShowPlayer1();
        ShowPlayer2();
    }

    void Update()
    {
        if (joycon == null || isLoading)
            return;

        Gamepad p1 = joycon.Player1Pad;
        Gamepad p2 = joycon.Player2Pad;

        if (p1 != null)
            CheckPlayer1(p1);

        if (p2 != null)
            CheckPlayer2(p2);

        // 両方が決定したらバトル開始
        if (player1Confirmed && player2Confirmed)
        {
            LoadBattleScene();
        }
    }

    void CheckPlayer1(Gamepad pad)
    {
        if (player1Confirmed)
            return;

        // Aボタンで1P決定
        if (pad.buttonSouth.wasPressedThisFrame)
        {
            player1Confirmed = true;
            Debug.Log("1P キャラクター決定！");
            return;
        }

        bool left =
            pad.dpad.left.isPressed ||
            pad.leftStick.x.ReadValue() < -0.5f;

        bool right =
            pad.dpad.right.isPressed ||
            pad.leftStick.x.ReadValue() > 0.5f;

        if (left && !p1LeftHeld)
            ChangePlayer1(-1);
        else if (right && !p1RightHeld)
            ChangePlayer1(1);

        p1LeftHeld = left;
        p1RightHeld = right;
    }

    void CheckPlayer2(Gamepad pad)
    {
        if (player2Confirmed)
            return;

        // Aボタンで2P決定
        if (pad.buttonSouth.wasPressedThisFrame)
        {
            player2Confirmed = true;
            Debug.Log("2P キャラクター決定！");
            return;
        }

        bool left =
            pad.dpad.left.isPressed ||
            pad.leftStick.x.ReadValue() < -0.5f;

        bool right =
            pad.dpad.right.isPressed ||
            pad.leftStick.x.ReadValue() > 0.5f;

        if (left && !p2LeftHeld)
            ChangePlayer2(-1);
        else if (right && !p2RightHeld)
            ChangePlayer2(1);

        p2LeftHeld = left;
        p2RightHeld = right;
    }

    void ChangePlayer1(int direction)
    {
        player1Index =
            (player1Index + direction + characterPrefabs.Length)
            % characterPrefabs.Length;

        ShowPlayer1();
    }

    void ChangePlayer2(int direction)
    {
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
        Player1_con p1 = character.GetComponent<Player1_con>();
        if (p1 != null)
            p1.enabled = false;

        Player2_con p2 = character.GetComponent<Player2_con>();
        if (p2 != null)
            p2.enabled = false;

        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void LoadBattleScene()
    {
        if (isLoading)
            return;

        // 選択データが正しいか確認
        if (characterPrefabs == null ||
            player1Index >= characterPrefabs.Length ||
            player2Index >= characterPrefabs.Length)
        {
            Debug.LogError("キャラクター選択データが不正です！");
            return;
        }

        // 選択したPrefabを保存
        CharacterSelectData.Player1Character =
            characterPrefabs[player1Index];

        CharacterSelectData.Player2Character =
            characterPrefabs[player2Index];

        Debug.Log(
            "1P選択キャラ: " +
            CharacterSelectData.Player1Character.name
        );

        Debug.Log(
            "2P選択キャラ: " +
            CharacterSelectData.Player2Character.name
        );

        isLoading = true;

        Debug.Log("両プレイヤー決定！バトル開始！");

        SceneManager.LoadScene(battleSceneName);
    }
}
