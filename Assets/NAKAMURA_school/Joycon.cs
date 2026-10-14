using UnityEngine;

public class Joycon : MonoBehaviour
{
    public Player_con player1;
    public Player_con player2;

    private int p1Joystick = -1;
    private int p2Joystick = -1;

    void Update()
    {
        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetKeyDown("joystick " + i + " button 1"))
            {
                if (p1Joystick == -1)
                {
                    p1Joystick = i;
                    player1.joystickNumber = i;

                    Debug.Log("1PŽQ‰Á");
                }
                else if (p2Joystick == -1 && i != p1Joystick)
                {
                    p2Joystick = i;
                    player2.joystickNumber = i;

                    Debug.Log("2PŽQ‰Á");
                }
            }
        }
    }
}