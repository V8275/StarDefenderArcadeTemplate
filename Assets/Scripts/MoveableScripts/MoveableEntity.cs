using UnityEngine;

public abstract class MoveableEntity : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;

    protected Vector3 targetPosition;
    protected bool isMoving = false;

    protected virtual void UpdateMovement()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            OnReachedTarget();
        }
    }

    protected abstract void OnReachedTarget();
}