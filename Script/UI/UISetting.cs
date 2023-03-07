using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : MonoBehaviour
{
    public AudioListener audioListener;
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SoundSwtich()
    {
        if (audioListener != null)
        audioListener.enabled = !audioListener.enabled;
    }

}
