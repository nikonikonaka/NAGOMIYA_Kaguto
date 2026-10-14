using UnityEngine;

public class Attack : MonoBehaviour
{
    public int joystickNumber = 1;

    public float normalSpeed = 5f;
    public float boostSpeed = 10f;

    private float holdTime = 0f;

    void Update()
    {
        float move = Input.GetAxisRaw(
            "J" + joystickNumber + "_Horizontal"
        );

        if (Mathf.Abs(move) > 0.3f)
        {
            holdTime += Time.deltaTime;

            float currentSpeed = normalSpeed;

            if (holdTime >= 1f)
            {
                currentSpeed = boostSpeed;
            }

            transform.Translate(
                Vector2.right * move * currentSpeed * Time.deltaTime
            );
        }
        else
        {
            holdTime = 0f;
        }
    }
}