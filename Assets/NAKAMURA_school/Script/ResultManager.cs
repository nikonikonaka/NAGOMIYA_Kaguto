using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Image winnerImage;

    void Start()
    {
        winnerImage.sprite =
            GameResultData.winnerSprite;
    }
}