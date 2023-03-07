using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
public class EnemyPool : MonoBehaviour
{
    [SerializeField] private bool collectionChecks = true;
    [SerializeField] private int maxPoolSize;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask m_MapLayer;
    private EnemyManager m_EnemyManager;
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
        m_EnemyManager = EnemyManager.Instance;
        if (m_Pool == null)
            m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
    }

    GameObject CreatePooledItem()
    {
        Vector3 des = m_EnemyManager.GetRamdomSpawnPoint();
        des.y = target.transform.position.y;
        des.x += Random.Range(-10, 10);
        des.z += Random.Range(-10, 10);

        var instance = Instantiate(target, des, Quaternion.identity);
        var returnToPool = instance.AddComponent<ReturnToEnemyPool>();
        returnToPool.pool = Pool;
        return instance;
    }

    void OnReturnedToPool(GameObject instance)
    {
        instance.GetComponent<NavMeshAgent>().enabled = false;
        m_EnemyManager.DecreaseEnemyNumber();
    }

    void OnTakeFromPool(GameObject instance)
    {
        Vector3 des = m_EnemyManager.GetRamdomSpawnPoint();
        des.y = target.transform.position.y;
        des.x += Random.Range(-10, 10);
        des.z += Random.Range(-10, 10);
        instance.transform.position = des;
        instance.GetComponent<NavMeshAgent>().enabled = true;
        m_EnemyManager.AddEnemyNumber();
    }

    void OnDestroyPoolObject(GameObject instance)
    {
        Destroy(instance);
    }
}

public class ReturnToEnemyPool : MonoBehaviour
{
    public IObjectPool<GameObject> pool;

    void OnDisable()
    {
        // Return to the pool
        pool.Release(gameObject);
    }
}
