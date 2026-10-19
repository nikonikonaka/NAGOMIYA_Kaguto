
using UnityEngine;
using UnityEngine.InputSystem;

public class Joycon : MonoBehaviour
{
    public Player1_con player1;
    public Player2_con player2;

    private Gamepad player1Pad;
    private Gamepad player2Pad;

    private bool player1Joined;
    private bool player2Joined;

    void Awake()
    {
        if (player1 != null)
            player1.enabled = false;

        if (player2 != null)
            player2.enabled = false;
    }

    void Update()
    {
        foreach (Gamepad pad in Gamepad.all)
        {
            if (!pad.buttonSouth.wasPressedThisFrame)
                continue;

            // ‚Ü‚¾Q‰Á‚µ‚Ä‚¢‚È‚¢1P‚ğ“o˜^
            if (!player1Joined)
            {
                player1Pad = pad;
                player1Joined = true;

                player1.SetPad(pad);
                player1.enabled = true;

                Debug.Log(
                    "[Q‰Á“o˜^] 1P: " +
                    pad.displayName +
                    " / ID=" + pad.deviceId
                );

                continue;
            }

            // 1P‚Æ‚Íˆá‚¤ƒRƒ“ƒgƒ[ƒ‰[‚Å2P‚ğ“o˜^
            if (!player2Joined &&
                pad.deviceId != player1Pad.deviceId)
            {
                player2Pad = pad;
                player2Joined = true;

                player2.SetPad(pad);
                player2.enabled = true;

                Debug.Log(
                    "[Q‰Á“o˜^] 2P: " +
                    pad.displayName +
                    " / ID=" + pad.deviceId
                );
            }
        }
    }
}
