using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    [Header("Pooling Settings")]
    [SerializeField] private T prefab;            
    [SerializeField] private int initialSize = 10;
    [SerializeField] private Transform parent;      

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

    public virtual T Get()
    {
        if (objects.Count == 0)
            AddObjectToPool();

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public virtual void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        objects.Enqueue(obj);
    }

    
}