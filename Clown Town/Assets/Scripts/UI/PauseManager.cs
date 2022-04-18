using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    public AudioMixer audioMixer;
    bool paused;
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Pause"))
            Pause();
    }

    public void MainMenu()
    {
        FaderCanvas.instance.GoAway("MainMenu");
    }


    public void Pause()
    {
        if (!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0)))
            return;
        paused = !paused;
        SelectionController.instance.canSelect = !paused;
        anim.SetBool("Pause", paused);
        audioMixer.SetFloat("MasterLowpass", (paused) ? 500f : 22000f);
    }

    public void PauseTime(int pause)
    {
        Time.timeScale = (pause == 0) ? 0f : 1f;
    }
}
