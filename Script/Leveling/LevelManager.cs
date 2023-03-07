using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private LevelData[] m_Datas;
    [SerializeField] private int m_CurrentWaveCount = 0;
    [SerializeField] private EnemyManager m_EnemyManager;
    private float m_TimeOffset;
    private bool m_StartCounting = false;
    public bool StartCounting => m_StartCounting;
    protected override void Awake()
    {
        base.Awake();
        m_CurrentWaveCount = 0;
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnGameOver += ResetStat;      
    }

    private void OnGameStart()
    {
        m_EnemyManager = EnemyManager.Instance;
        InvokeRepeating("StartWave", 3, 20);
        m_TimeOffset = Time.time;
    }

    private void StartWave()
    {
        LevelData data = m_Datas[m_CurrentWaveCount];
        m_CurrentWaveCount++;
        if(m_CurrentWaveCount >= m_Datas.Length)
        {
            StartCoroutine(FinalWaveCountDown());
            CancelInvoke("StartWave");
        }
        int waitTime = (int)(Time.time - m_TimeOffset - data.StartTime);
        for (int i = 0; i < data.EnemySpawnNumber.Length; i++)
        {
            if (data.EnemySpawnNumber[i] <= 0) continue;
            m_EnemyManager.SetSpawner(data.EnemySpawnNumber[i], data.SpawnRate[i],
                waitTime, (Enemy)(i + 1));
        }
    }

    IEnumerator FinalWaveCountDown()
    {
        m_StartCounting = true;
        yield return new WaitForSeconds(180);
        GameManager.Instance.GameOver();
    }

    private void ResetStat()
    {
        m_CurrentWaveCount = 0;
        m_StartCounting = false;
        CancelInvoke("StartWave");
    }
}
