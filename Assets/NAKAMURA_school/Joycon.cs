
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

    void Start()
    {
        // Aƒ{ƒ^ƒ“‚ğ‰Ÿ‚·‘O‚ÉAÚ‘±‡‚Å“o˜^‚·‚é
        AssignControllers();
    }

    void AssignControllers()
    {
        // Ú‘±‚³‚ê‚Ä‚¢‚éƒRƒ“ƒgƒ[ƒ‰[‚ğæ“¾
        var gamepads = Gamepad.all;

        // 1‘ä–Ú‚ğ1P‚É“o˜^
        if (gamepads.Count >= 1)
        {
            player1Pad = gamepads[0];
            player1Joined = true;

            if (player1 != null)
            {
                player1.SetPad(player1Pad);
                player1.enabled = true;
            }

            Debug.Log(
                "[Ú‘±“o˜^] 1P: " +
                player1Pad.displayName +
                " / ID=" + player1Pad.deviceId
            );
        }

        // 2‘ä–Ú‚ğ2P‚É“o˜^
        if (gamepads.Count >= 2)
        {
            player2Pad = gamepads[1];
            player2Joined = true;

            if (player2 != null)
            {
                player2.SetPad(player2Pad);
                player2.enabled = true;
            }

            Debug.Log(
                "[Ú‘±“o˜^] 2P: " +
                player2Pad.displayName +
                " / ID=" + player2Pad.deviceId
            );
        }
    }
}
/* æ‚ÉÚ‘±‡‚Å“o˜^‚·‚éê‡‚ÌƒR[ƒh—á
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
*/