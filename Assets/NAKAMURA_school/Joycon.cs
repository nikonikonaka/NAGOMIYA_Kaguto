
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joycon : MonoBehaviour
{
    public Player_con player1;
    public Player2_con player2;

    private Gamepad player1Pad;
    private Gamepad player2Pad;

    private bool player1Joined = false;
    private bool player2Joined = false;

    public Gamepad GetPlayer1Pad() => player1Pad;
    public Gamepad GetPlayer2Pad() => player2Pad;

    void Start()
    {
        player1.enabled = false;
        player2.enabled = false;
    }

    void Update()
    {

       

        foreach (Gamepad pad in Gamepad.all)
        {
            if (!pad.buttonSouth.wasPressedThisFrame)
                continue;

            // 1P参加
            if (!player1Joined)
            {
                player1Pad = pad;
                player1.SetPad(pad);
                player1Joined = true;
                player1.enabled = true;

                Debug.Log(
                    $"1P参加: {pad.displayName}, ID={pad.deviceId}"
                );
                continue;
            }

            // 2P参加（1Pとは異なるデバイス）
            if (!player2Joined && pad.deviceId != player1Pad.deviceId)
            {
                player2Pad = pad;
                player2.SetPad(pad);
                player2Joined = true;
                player2.enabled = true;

                Debug.Log(
                    $"2P参加: {pad.displayName}, ID={pad.deviceId}"
                );
            }
        }
       
if (player1Pad != null && player2Pad != null)
        {
            float input1 = player1Pad.leftStick.x.ReadValue();
            float input2 = player2Pad.leftStick.x.ReadValue();

            Debug.Log(
                $"1P ID={player1Pad.deviceId}, 入力={input1:F2} | " +
                $"2P ID={player2Pad.deviceId}, 入力={input2:F2}"
            );

            Debug.DrawRay(
                player1.transform.position,
                Vector2.right * input1,
                Color.blue
            );

            Debug.DrawRay(
                player2.transform.position,
                Vector2.right * input2,
                Color.red
            );
        }
    }
}
