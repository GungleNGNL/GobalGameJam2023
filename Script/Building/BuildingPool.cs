using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BuildingPool : MonoBehaviour
{
    [SerializeField] private bool collectionChecks = true;
    [SerializeField] private int maxPoolSize;
    [SerializeField] private GameObject target;
    IObjectPool<GameObject> m_Pool;
    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
            }
            return m_Pool;
        }
    }

    private void Awake()
    {
        if (m_Pool == null)
            m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
    }

    GameObject CreatePooledItem()
    {
        var instance = Instantiate(target);
        var returnToPool = instance.AddComponent<ReturnToBuildingPool>();
        returnToPool.pool = Pool;
        return instance;
    }

    void OnReturnedToPool(GameObject instance)
    {
        instance.GetComponent<BoxCollider>().enabled = false;
    }

    void OnTakeFromPool(GameObject instance)
    {
        instance.GetComponent<BoxCollider>().enabled = true;
    }

    void OnDestroyPoolObject(GameObject instance)
    {
        Destroy(instance);
    }
}

public class ReturnToBuildingPool : MonoBehaviour
{
    public IObjectPool<GameObject> pool;

    void OnDisable()
    {
        // Return to the pool
        pool.Release(gameObject);
    }
}
