using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private IntVariable m_Energy;
    [SerializeField] private int m_StartEnergy;
    public UnityAction OnNewGame;
    public UnityAction OnGameStart;
    public UnityAction OnGameOver;
    public UnityAction OnGamePause;

    public void NewGame()
    {
        OnNewGame?.Invoke();
    }

    public void GameStart()
    {
        m_Energy.Value = m_StartEnergy;
        OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
        SceneManager.LoadScene(0);
    }

    public void GamePause()
    {
        OnGamePause?.Invoke();
    }
}
