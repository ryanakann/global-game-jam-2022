using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{

    public void LoadMainMenu()
    {
        FaderCanvas.instance.GoAway("MainMenu");
    }
}
