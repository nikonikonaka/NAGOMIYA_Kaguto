using UnityEngine;
using UnityEngine.InputSystem;

public class TestJoycon : MonoBehaviour
{
    public Player1_con player1;
    public Player2_con player2;

    private Gamepad player1Pad;
    private Gamepad player2Pad;

    public Gamepad Player1Pad => player1Pad;
    public Gamepad Player2Pad => player2Pad;

    void Start()
    {
        var pads = Gamepad.all;

        if (pads.Count > 0)
        {
            player1Pad = pads[0];

            player1.SetPad(player1Pad);

            Debug.Log("1P“o˜^: " + player1Pad.displayName);
        }

        if (pads.Count > 1)
        {
            player2Pad = pads[1];

            player2.SetPad(player2Pad);

            Debug.Log("2P“o˜^: " + player2Pad.displayName);
        }
    }
}