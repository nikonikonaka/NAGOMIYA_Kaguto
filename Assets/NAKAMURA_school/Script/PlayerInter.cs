using UnityEngine.InputSystem;

public interface IPlayerController
{
    Gamepad myPad { get; }
    bool IsFacingLeft();
}