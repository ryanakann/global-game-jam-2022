using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : Singleton<Radio>
{
    public List<AudioClip> musicClips;
    public List<AudioClip> advertClips;

    public AudioClip staticClip;

    private AudioSource audioSource;    
    private Coroutine coroutine;

    public List<AudioClip> clips;
    private int clipIndex;
    private float clipTime;

    GameObject bar1, bar2, bar3;

    private void Start()
    {
        bar1 = transform.FindDeepChild("Bar1").gameObject;
        bar2 = transform.FindDeepChild("Bar2").gameObject;
        bar3 = transform.FindDeepChild("Bar3").gameObject;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (!audioSource.isPlaying) audioSource.Play();

        clipIndex = 0;
        clipTime = 0f;
        
        ShuffleClips();
        ChangeChannel();
        SetVolume(1);
    }

    public void SetVolume(int level)
    {
        switch (level)
        {
            case 0:
                audioSource.volume = 0f;
                bar1.SetActive(false);
                bar2.SetActive(false);
                bar3.SetActive(false);
                break;
            case 1:
                audioSource.volume = 0.2f;
                bar1.SetActive(true);
                bar2.SetActive(false);
                bar3.SetActive(false);
                break;
            case 2:
                audioSource.volume = 0.6f;
                bar1.SetActive(true);
                bar2.SetActive(true);
                bar3.SetActive(false);
                break;
            case 3:
                audioSource.volume = 1f;
                bar1.SetActive(true);
                bar2.SetActive(true);
                bar3.SetActive(true);
                break;
        }
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    private void ShuffleClips()
    {
        clips = new List<AudioClip>();
        musicClips.ForEach(clip => clips.Add(clip));
        advertClips.ForEach(clip => clips.Add(clip));
        clips.Shuffle();
    }

    public void ChangeChannel()
    {
        if (coroutine == null && clips != null && clips.Count > 0)
        {
            coroutine = StartCoroutine(ChangeChannelCR());
        }
    }

    private IEnumerator ChangeChannelCR()
    {
        audioSource.clip = staticClip;
        audioSource.time = 0f;
        if (!audioSource.isPlaying) audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        clipIndex = (clipIndex + 1) % clips.Count;
        audioSource.clip = clips[clipIndex];
        audioSource.time = Mathf.Min(clipTime % audioSource.clip.length, audioSource.clip.length - 0.01f);
        if (!audioSource.isPlaying) audioSource.Play();
        coroutine = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeChannel();
        }
        clipTime += Time.deltaTime;
    }
}
