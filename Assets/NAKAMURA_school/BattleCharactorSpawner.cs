
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleCharacterSpawner : MonoBehaviour
{
    [Header("コントローラー")]
    public Joycon joycon;

    [Header("生成位置")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("HPバー")]
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private GameObject player1Object;
    private GameObject player2Object;

    IEnumerator Start()
    {
        // Joyconのコントローラー登録を待つ
        yield return null;

        if (joycon == null ||
            player1SpawnPoint == null ||
            player2SpawnPoint == null)
        {
            Debug.LogError("SpawnerのInspector設定を確認してください");
            yield break;
        }

        Gamepad p1 = joycon.Player1Pad;
        Gamepad p2 = joycon.Player2Pad;

        GameObject prefab1 =
            CharacterSelectData.Player1Character;

        GameObject prefab2 =
            CharacterSelectData.Player2Character;

        if (prefab1 == null || prefab2 == null)
        {
            Debug.LogError("選択されたPrefabがありません");
            yield break;
        }

        if (p1 == null || p2 == null)
        {
            Debug.LogError("コントローラーが2台必要です");
            yield break;
        }

        // 1P生成
        player1Object = Instantiate(
            prefab1,
            player1SpawnPoint.position,
            player1SpawnPoint.rotation
        );

        player1Object.name = "BattlePlayer1";

        // 2P生成
        player2Object = Instantiate(
            prefab2,
            player2SpawnPoint.position,
            player2SpawnPoint.rotation
        );

        player2Object.name = "BattlePlayer2";

        // 操作割り当て
        SetupPlayer(player1Object, p1, true);
        SetupPlayer(player2Object, p2, false);

        Debug.Log("1P・2PのPrefab生成完了！");

        // 1PのHPバー設定
        Player_Health hp1 =
            player1Object.GetComponent<Player_Health>();

        if (hp1 != null)
        {
            hp1.SetHealthBar(player1HealthBar);
        }

        // 2PのHPバー設定
        Player_Health hp2 =
            player2Object.GetComponent<Player_Health>();

        if (hp2 != null)
        {
            hp2.SetHealthBar(player2HealthBar);
        }
    }

    void SetupPlayer(
        GameObject character,
        Gamepad pad,
        bool isPlayer1)
    {
        Player1_con p1 =
            character.GetComponent<Player1_con>();

        Player2_con p2 =
            character.GetComponent<Player2_con>();

        if (p1 != null)
            p1.enabled = false;

        if (p2 != null)
            p2.enabled = false;

        // 1P操作を割り当て
        if (isPlayer1)
        {
            if (p1 != null)
            {
                p1.enabled = true;
                p1.SetPad(pad);
            }
            else if (p2 != null)
            {
                p2.enabled = true;
                p2.SetPad(pad);
            }
            else
            {
                Debug.LogError(
                    character.name + " に操作スクリプトがありません"
                );
            }
        }
        // 2P操作を割り当て
        else
        {
            if (p2 != null)
            {
                p2.enabled = true;
                p2.SetPad(pad);
            }
            else if (p1 != null)
            {
                p1.enabled = true;
                p1.SetPad(pad);
            }
            else
            {
                Debug.LogError(
                    character.name + " に操作スクリプトがありません"
                );
            }
        }
    }
}
