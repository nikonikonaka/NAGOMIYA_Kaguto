using UnityEngine;

public class ChildPositionFlip : MonoBehaviour
{
    public Player1_con player1;
    public Player2_con player2;

    private Vector3 defaultLocalPos;

    void Start()
    {
        defaultLocalPos = transform.localPosition;
    }

    void Update()
    {
        bool left = false;

        if (player1 != null)
        {
            left = player1.IsFacingLeft();
        }
        else if (player2 != null)
        {
            left = player2.IsFacingLeft();
        }
        else
        {
            return;
        }

        Vector3 pos = defaultLocalPos;

        // Å©Ç±Ç±Çãtì]
        pos.x = left
            ? Mathf.Abs(defaultLocalPos.x)
            : -Mathf.Abs(defaultLocalPos.x);

        transform.localPosition = pos;
    }
}