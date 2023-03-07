using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    public void OnClick()
    {
        //GameManager.Instance.GameStart();
        SceneManager.LoadScene(1);
    }
}
