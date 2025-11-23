using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager instance;
    public static UpdateManager Instance => instance;

    private readonly List<Action> updateActions = new List<Action>();
    private readonly List<Action> fixedUpdateActions = new List<Action>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void RegisterUpdate(Action action) => updateActions.Add(action);
    public void UnregisterUpdate(Action action) => updateActions.Remove(action);

    public void RegisterFixedUpdate(Action action) => fixedUpdateActions.Add(action);
    public void UnregisterFixedUpdate(Action action) => fixedUpdateActions.Remove(action);

    private void Update()
    {
        for (int i = updateActions.Count - 1; i >= 0; i--)
        {
            updateActions[i]?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        for (int i = fixedUpdateActions.Count - 1; i >= 0; i--)
        {
            fixedUpdateActions[i]?.Invoke();
        }
    }
}