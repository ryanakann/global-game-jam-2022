using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Play()
    {
        anim.SetTrigger("Play");
    }

    public void Intro(bool explain)
    {
        ExplainerManager.instance.explain = explain;
        FaderCanvas.instance.GoAway("ClownIntro");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
