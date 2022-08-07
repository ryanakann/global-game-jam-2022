using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FaderCanvas : PersistentSingleton<FaderCanvas>
{
    Animator anim;
    public AudioMixer audioMixer;
    [HideInInspector]
    public bool fading;
    public GameEvent finishedFading;

    public void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Fade()
    {
        anim.SetBool("Fade", true);
        StartCoroutine(Quiet());
    }

    IEnumerator Quiet()
    {
        // FX_Spawner.instance.SpawnFX(FXType.ScreenOut);
        float vol = 1.5f; // 0 : -40
        while (vol > 0f)
        {
            audioMixer.SetFloat("MasterVolume", vol / 1.5f * 40f -40f);
            vol -= Time.deltaTime;
            yield return null;
        }
    }

    public void Unfade()
    {
        audioMixer.SetFloat("MasterVolume", 0f);
        audioMixer.SetFloat("MasterLowpass", 22000f);
        Time.timeScale = 1f;
        anim.SetBool("Fade", false);
        StartCoroutine(Unquiet());
    }

    IEnumerator Unquiet()
    {
        FX_Spawner.instance.ResetCounter();
        // FX_Spawner.instance.SpawnFX(FXType.ScreenIn);
        float vol = 0f; // 0 : -40
        while (vol < 1.5f)
        {
            audioMixer.SetFloat("MasterVolume", vol / 1.5f * 40f - 40f);
            vol += Time.deltaTime;
            yield return null;
        }
        fading = false;
        finishedFading?.Invoke();
    }

    public void GoAway(string scene)
    {
        StartCoroutine(CoGoAway(scene));
    }

    IEnumerator CoGoAway(string scene)
    {
        fading = true;
        Fade();
        yield return new WaitForSeconds(3f);
        var op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone) yield return null;
        Unfade();
    }
}
