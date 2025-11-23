using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void Initialize()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count == 0)
            CreateNewObject();

        var obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.Enqueue(obj);
    }

    private void CreateNewObject()
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        var poolable = obj.GetComponent<IPoolable>();
        poolable?.SetPool(this);

        pool.Enqueue(obj);
    }
}

public interface IPoolable
{
    void SetPool(ObjectPool pool);
    void ReturnToPool();
}