
using UnityEngine;
using UnityEngine.InputSystem;

public class Joycon : MonoBehaviour
{
    // 1P・2Pのコントローラー
    private Gamepad player1Pad;
    private Gamepad player2Pad;

    // 他のスクリプトから取得するためのプロパティ
    public Gamepad Player1Pad => player1Pad;
    public Gamepad Player2Pad => player2Pad;

    void Start()
    {
        AssignControllers();
    }

    // 接続順でコントローラーを割り当てる
    void AssignControllers()
    {
        var gamepads = Gamepad.all;

        // 1P
        if (gamepads.Count >= 1)
        {
            player1Pad = gamepads[0];

            Debug.Log(
                "[接続登録] 1P: " +
                player1Pad.displayName +
                " / ID=" + player1Pad.deviceId
            );
        }
        else
        {
            Debug.LogWarning(
                "1P用コントローラーが接続されていません"
            );
        }

        // 2P
        if (gamepads.Count >= 2)
        {
            player2Pad = gamepads[1];

            Debug.Log(
                "[接続登録] 2P: " +
                player2Pad.displayName +
                " / ID=" + player2Pad.deviceId
            );
        }
        else
        {
            Debug.LogWarning(
                "2P用コントローラーが接続されていません"
            );
        }
    }
}


/*
using UnityEngine;
using UnityEngine.InputSystem;

public class Joycon : MonoBehaviour
{
   

    public Player1_con player1;
    public Player2_con player2;

    private Gamepad player1Pad;
    private Gamepad player2Pad;

    public Gamepad Player1Pad => player1Pad;
    public Gamepad Player2Pad => player2Pad;

    void Awake()
    {
        // 最初は両プレイヤーの操作を停止
        if (player1 != null)
            player1.enabled = false;

        if (player2 != null)
            player2.enabled = false;
    }

    void Start()
    {
        // Aボタンを押さずに接続順で割り当てる
        AssignControllers();
    }

    void AssignControllers()
    {
        var gamepads = Gamepad.all;

        // 1台目を1Pに割り当て
        if (gamepads.Count >= 1)
        {
            player1Pad = gamepads[0];

            if (player1 != null)
            {
                player1.SetPad(player1Pad);
                player1.enabled = true;
            }

            Debug.Log(
                "[接続登録] 1P: " +
                player1Pad.displayName +
                " / ID=" + player1Pad.deviceId
            );
        }
        else
        {
            Debug.LogWarning("1P用コントローラーが接続されていません");
        }

        // 2台目を2Pに割り当て
        if (gamepads.Count >= 2)
        {
            player2Pad = gamepads[1];

            if (player2 != null)
            {
                player2.SetPad(player2Pad);
                player2.enabled = true;
            }

            Debug.Log(
                "[接続登録] 2P: " +
                player2Pad.displayName +
                " / ID=" + player2Pad.deviceId
            );
        }
        else
        {
            Debug.LogWarning("2P用コントローラーが接続されていません");
        }
    }
}
*/
/* 先に接続順で登録する場合のコード例
 * public Player1_con player1;
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

           // まだ参加していない1Pを登録
           if (!player1Joined)
           {
               player1Pad = pad;
               player1Joined = true;

               player1.SetPad(pad);
               player1.enabled = true;

               Debug.Log(
                   "[参加登録] 1P: " +
                   pad.displayName +
                   " / ID=" + pad.deviceId
               );

               continue;
           }

           // 1Pとは違うコントローラーで2Pを登録
           if (!player2Joined &&
               pad.deviceId != player1Pad.deviceId)
           {
               player2Pad = pad;
               player2Joined = true;

               player2.SetPad(pad);
               player2.enabled = true;

               Debug.Log(
                   "[参加登録] 2P: " +
                   pad.displayName +
                   " / ID=" + pad.deviceId
               );
           }
       }
   }
}
*/