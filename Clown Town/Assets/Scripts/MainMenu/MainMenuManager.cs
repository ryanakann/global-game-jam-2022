using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        FaderCanvas.instance.GoAway("ClownIntro");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
