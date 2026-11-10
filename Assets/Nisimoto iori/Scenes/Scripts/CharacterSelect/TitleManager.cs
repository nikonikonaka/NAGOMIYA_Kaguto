using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("メニューボタン")]
    public Button startButton;
    public Button optionButton;
    public Button quitButton;

    void Start()
    {
        // 最初にSTARTを選択状態にする
        EventSystem.current.SetSelectedGameObject(
            startButton.gameObject
        );
    }

    // ゲーム開始
    public void GameStart()
    {
        SceneManager.LoadScene("select");
    }

    // オプション
    public void OpenOption()
    {
        Debug.Log("オプション画面は後で実装！");
    }

    // ゲーム終了
    public void GameQuit()
    {
        Debug.Log("ゲームを終了します");

        Application.Quit();
    }
}
