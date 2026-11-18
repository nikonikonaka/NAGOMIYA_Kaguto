using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DemodoriManager : MonoBehaviour
{
    [Header("メニューボタン")]
    public Button startButton;
 

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
        SceneManager.LoadScene("start");
    }

  
}