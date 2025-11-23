using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MoveableEntity
{
    private bool mobileMovement = false;

    void Start()
    {
        mobileMovement = Application.isMobilePlatform;
        UpdateManager.Instance.RegisterUpdate(OnUpdate);
    }

    private void OnUpdate()
    {
        HandleInput();
        UpdateMovement();
    }

    private void HandleInput()
    {
        if (mobileMovement)
        {
            if (Touchscreen.current?.primaryTouch.press.isPressed == true)
                SetTargetPosition(Touchscreen.current.primaryTouch.position.ReadValue());
        }
        else
        {
            if (Mouse.current?.leftButton.isPressed == true)
                SetTargetPosition(Mouse.current.position.ReadValue());
        }
    }

    private void SetTargetPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.forward, transform.position.z);

        if (groundPlane.Raycast(ray, out float enter))
        {
            targetPosition = ray.GetPoint(enter);
            isMoving = true;
        }
    }

    protected override void OnReachedTarget()
    {
        isMoving = false;
    }

    private void OnDestroy()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
    }
}