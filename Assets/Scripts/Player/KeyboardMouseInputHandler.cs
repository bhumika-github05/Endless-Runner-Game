using UnityEngine;

public class KeyboardMouseInputHandler : IInputHandler
{
    private readonly IMovementHandler movementHandler;
    private readonly PlayerWeaponHandler weaponHandler;
    private readonly float swipeThreshold;

    private Vector2 startTouch;
    private bool isSwiping;

    public KeyboardMouseInputHandler(IMovementHandler movementHandler, PlayerWeaponHandler weaponHandler, float swipeThreshold = 10f)
    {
        this.movementHandler = movementHandler;
        this.weaponHandler = weaponHandler;
        this.swipeThreshold = swipeThreshold;
    }

    public void ProcessInput()
    {
        HandleKeyboard();
        HandleSwipe();
    }

    private void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) movementHandler.Roll();
        if (Input.GetKeyDown(KeyCode.UpArrow)) movementHandler.Jump();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) movementHandler.MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) movementHandler.MoveRight();

        // Shoot with spacebar if rifle equipped
        if (Input.GetKeyDown(KeyCode.Space)) weaponHandler?.Shoot();
    }

    private void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 endTouch = (Vector2)Input.mousePosition;
            isSwiping = false;

            Vector2 delta = endTouch - startTouch;

            if (delta.magnitude > swipeThreshold)
            {
                Vector2 dir = delta.normalized;

                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    if (dir.x > 0) movementHandler.MoveRight();
                    else movementHandler.MoveLeft();
                }
                else
                {
                    if (dir.y > 0) movementHandler.Jump();
                    else movementHandler.Roll();
                }
            }
            else
            {
                weaponHandler?.Shoot();
            }
        }
    }
}