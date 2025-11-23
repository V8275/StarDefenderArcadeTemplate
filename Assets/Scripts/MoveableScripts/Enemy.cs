using UnityEngine;

public class Enemy : MoveableEntity, IPoolable
{
    [SerializeField] 
    private LineRenderer linePath;
    [SerializeField]
    private int scoreCost = 1;

    private Vector3[] targetPoints = new Vector3[0];
    private int currentPoint = 0;
    private ObjectPool pool;
    private bool isInitialized = false;

    public int ScoreCost
    {
        get { return scoreCost; } 
    }

    public void SetPatrolPath(LineRenderer path)
    {
        linePath = path;
        InitializePath();
    }

    private void InitializePath()
    {
        if (linePath == null || isInitialized) return;

        targetPoints = new Vector3[linePath.positionCount];
        for (int i = 0; i < linePath.positionCount; i++)
            targetPoints[i] = linePath.GetPosition(i);

        if (targetPoints.Length > 0)
        {
            currentPoint = 0;
            SetTargetPosition();
        }

        UpdateManager.Instance?.RegisterUpdate(OnUpdate);

        isInitialized = true;
    }

    private void OnUpdate()
    {
        UpdateMovement();
    }

    protected override void OnReachedTarget()
    {
        currentPoint++;
        SetTargetPosition();
    }

    private void SetTargetPosition()
    {
        if (currentPoint >= targetPoints.Length)
            currentPoint = 0;

        targetPosition = targetPoints[currentPoint];
        isMoving = true;
    }

    public void SetPool(ObjectPool objectPool) => pool = objectPool;

    public void ReturnToPool()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
        currentPoint = 0;
        isMoving = false;
        isInitialized = false;
        pool?.ReturnObject(gameObject);
    }

    private void OnEnable()
    {
        if (linePath != null)
            InitializePath();
    }

    private void OnDisable()
    {
        UpdateManager.Instance?.UnregisterUpdate(OnUpdate);
        isInitialized = false;
    }
}