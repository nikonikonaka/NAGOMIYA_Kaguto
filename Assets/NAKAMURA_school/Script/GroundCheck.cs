using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGround;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}