using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FaderCanvas : PersistentSingleton<FaderCanvas>
{
    Animator anim;
    public AudioMixer audioMixer;

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
        float vol = 0f; // 0 : -40
        while (vol < 1.5f)
        {
            audioMixer.SetFloat("MasterVolume", vol / 1.5f * 40f - 40f);
            vol += Time.deltaTime;
            yield return null;
        }
    }

    public void GoAway(string scene)
    {
        StartCoroutine(CoGoAway(scene));
    }

    IEnumerator CoGoAway(string scene)
    {
        Fade();
        { yield return new WaitForSeconds(3f); }
        var op = SceneManager.LoadSceneAsync(scene);
        Unfade();
    }
}
