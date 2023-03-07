using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private EnemyPool m_NormalPool, m_SpitterPool, m_BoomerPool, m_SnailTankPool, m_SporePool;
    [SerializeField] private Transform[] m_SpawnPoint;
    [SerializeField] private EnemySpawner m_NormalSpawner, m_SpitterSpawner, m_BoomerSpawner, m_SnailTankSpawner, m_SporeSpawner;
    [SerializeField] private int m_EnemyCount = 0;

    protected override void Awake()
    {
        
    }

    public IObjectPool<GameObject> GetPool(Enemy type)
    {
        switch (type)
        {
            case Enemy.Normal:
                return m_NormalPool.Pool;
            case Enemy.Spitter:
                return m_SpitterPool.Pool;
            case Enemy.Boomer:
                return m_BoomerPool.Pool;
            case Enemy.SnailTank:
                return m_SnailTankPool.Pool;
            case Enemy.Spore:
                return m_SporePool.Pool;
            default:
                Debug.LogError("Enemy type missing");
                return null;
        }
    }

    public Vector3 GetRamdomSpawnPoint()
    {
        int index = Random.Range(0, m_SpawnPoint.Length - 1);
        Vector3 result = transform.TransformPoint(m_SpawnPoint[index].position);
        return result;
    }

    public void SetSpawner(int number, float rate, int waitTime, Enemy type)
    {
        switch (type) 
        {
            case Enemy.Normal:
                m_NormalSpawner.SetSpawner(number, rate, waitTime);
                break;
            case Enemy.Spitter:
                m_SpitterSpawner.SetSpawner(number, rate, waitTime);
                break;
            case Enemy.Boomer:
                m_BoomerSpawner.SetSpawner(number, rate, waitTime);
                break;
            case Enemy.SnailTank:
                m_SnailTankSpawner.SetSpawner(number, rate, waitTime);
                break;
            case Enemy.Spore:
                m_SporeSpawner.SetSpawner(number, rate, waitTime);
                break;
            default:
                Debug.LogError("Enemy type missing");
                break;
        }
    }

    public void AddEnemyNumber()
    {
        m_EnemyCount++; 
    }

    public void DecreaseEnemyNumber()
    {
        m_EnemyCount--;
        if(m_EnemyCount == 0)
        {
            if (LevelManager.Instance.StartCounting)
                GameManager.Instance.GameOver();
        }
    }
}

public enum Enemy 
{ 
    None = 0,
    Normal,
    Spitter,
    Boomer,
    SnailTank,
    Spore,
}

