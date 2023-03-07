using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float m_SpawnPeriod;
    [SerializeField] int m_SpawnNumber;
    private IObjectPool<GameObject> m_Pool;
    private EnemyManager m_EnemyManager;
    [SerializeField] Enemy m_SpawnType;
    private void Awake()
    {
        m_EnemyManager = EnemyManager.Instance;
    }

    private void Start()
    {
        m_Pool = m_EnemyManager.GetPool(m_SpawnType);
        GameManager.Instance.OnGameOver += ResetStat;
    }

    public void SetSpawner(int number, float spawnPeriod, float startSpawnDelay)
    {
        m_SpawnNumber += number;
        m_SpawnPeriod = spawnPeriod;
        StartCoroutine(CountingToSpawningSpawnEnemy(startSpawnDelay));
    }

    IEnumerator CountingToSpawningSpawnEnemy(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(StartSpawningSpawnEnemy(m_SpawnPeriod));
    }

    IEnumerator StartSpawningSpawnEnemy(float spawnPeriod)
    {
        while (m_SpawnNumber > 0)
        {
            SpawnEnemy();
            m_SpawnNumber--;
            yield return new WaitForSeconds(spawnPeriod);
        }     
    }

    private void SpawnEnemy()
    {
        GameObject obj = m_Pool.Get();
        obj.SetActive(true);
    }

    private void ResetStat()
    {
        StopAllCoroutines();
        m_SpawnNumber = 0;
        m_SpawnPeriod = 0;
    }
}
