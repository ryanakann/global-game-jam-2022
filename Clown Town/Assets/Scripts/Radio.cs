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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (!audioSource.isPlaying) audioSource.Play();

        clipIndex = 0;
        clipTime = 0f;
        
        ShuffleClips();
        ChangeChannel();
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
