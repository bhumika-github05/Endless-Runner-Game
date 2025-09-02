using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    [Header("Pooling Settings")]
    [SerializeField] private T prefab;              // assign prefab in Inspector
    [SerializeField] private int initialSize = 10;
    [SerializeField] private Transform parent;      // optional parent for instances

    private readonly Queue<T> objects = new Queue<T>();

    protected virtual void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab not assigned for {typeof(T)} pool on {gameObject.name}");
            return;
        }

        if (parent == null)
            parent = transform;

        // Pre-instantiate
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private T AddObjectToPool()
    {
        T newObj = Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        objects.Enqueue(newObj);
        return newObj;
    }

    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    public virtual T Get()
    {
        if (objects.Count == 0)
            AddObjectToPool();

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Return an object back to the pool.
    /// </summary>
    public virtual void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        objects.Enqueue(obj);
    }

    /// <summary>
    /// Destroy all objects in pool (useful on hard reset).
    /// </summary>
    public void Clear()
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                Destroy(obj.gameObject);
        }
        objects.Clear();
    }
}