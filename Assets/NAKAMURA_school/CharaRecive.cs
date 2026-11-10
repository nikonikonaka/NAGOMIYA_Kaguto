using UnityEngine;

public class CharacterDataReceiver : MonoBehaviour
{
    void Start()
    {
        Debug.Log("===== キャラクター受け取り確認 =====");

        if (CharacterSelectData.Player1Character != null)
        {
            Debug.Log(
                "1P受け取り成功: " +
                CharacterSelectData.Player1Character.name
            );
        }
        else
        {
            Debug.LogError("1Pのキャラクターを受け取れていません！");
        }

        if (CharacterSelectData.Player2Character != null)
        {
            Debug.Log(
                "2P受け取り成功: " +
                CharacterSelectData.Player2Character.name
            );
        }
        else
        {
            Debug.LogError("2Pのキャラクターを受け取れていません！");
        }
    }
}