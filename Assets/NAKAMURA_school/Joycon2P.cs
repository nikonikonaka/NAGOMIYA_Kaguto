using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Joycon2P : MonoBehaviour
{
    public Joycon player1Manager;

    public PlayerInput player2;
    public Player2_con player2Control;

    private Gamepad player2Pad;
    private bool player2Joined;

    void Start()
    {
        player2Control.enabled = false;
        player2Control.joined = false;

        player2.DeactivateInput();
    }

    void Update()
    {
        foreach (Gamepad pad in Gamepad.all)
        {
            if (pad.buttonSouth.wasPressedThisFrame)
            {
                JoinPlayer2(pad);
            }
        }
    }

    void JoinPlayer2(Gamepad pad)
    {
        if (player2Joined)
            return;

        // 1Pで使っているパッドは無視
        if (pad == player1Manager.GetPlayer1Pad())
            return;

        player2Pad = pad;

        InputUser.PerformPairingWithDevice(
            pad,
            player2.user
        );

        player2Control.joined = true;
        player2Control.enabled = true;

        player2.ActivateInput();

        player2Joined = true;

        Debug.Log("2P参加");
    }
}