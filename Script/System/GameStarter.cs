using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.GameStart();
    }
}
