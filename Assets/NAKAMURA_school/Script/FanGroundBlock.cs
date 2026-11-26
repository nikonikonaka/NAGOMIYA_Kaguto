using UnityEngine;

public class FanGroundBlock : MonoBehaviour
{
    public bool boostTouchGround;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            boostTouchGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            boostTouchGround = false;
        }
    }
}