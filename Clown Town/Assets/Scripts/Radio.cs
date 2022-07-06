using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RadioChannel
{
    public string channelName;
    int currentTrack;
    public List<AudioClip> tracks = new List<AudioClip>();
    public List<AudioClip> trackToAdvert = new List<AudioClip>();
    public List<AudioClip> advertToTrack = new List<AudioClip>();

    public AudioClip NextTrack()
    {
        var track = tracks[currentTrack];
        currentTrack++;
        if (currentTrack >= tracks.Count)
        {
            currentTrack = 0;
            tracks.Shuffle();
        }
        return track;
    }
}

[RequireComponent(typeof(AudioSource))]
public class Radio : Singleton<Radio>
{
    public AudioClip introduction;
    public RadioChannel advertChannel;

    public List<AudioClip> musicClips;
    public List<AudioClip> advertClips;

    Vector2 advertRange = new Vector2(1, 3);
    Vector2 songRange = new Vector2(2, 4);
    int advertCount, songCount;

    public AudioClip staticClip;

    RadioChannel currentChannel;
    int channelIndex;

    public List<RadioChannel> radioChannels = new List<RadioChannel>();

    private AudioSource audioSource;    
    private Coroutine coroutine;

    bool paused, song;

    public List<AudioClip> clips;
    private int clipIndex;
    private float clipTime;

    GameObject bar1, bar2, bar3;

    bool notfuckingreadyyet = true;

    private void Start()
    {
        bar1 = transform.FindDeepChild("Bar1").gameObject;
        bar2 = transform.FindDeepChild("Bar2").gameObject;
        bar3 = transform.FindDeepChild("Bar3").gameObject;
        audioSource = GetComponent<AudioSource>();
        //audioSource.loop = true;
        //if (!audioSource.isPlaying) audioSource.Play();

        clipIndex = 0;
        clipTime = 0f;

        if (radioChannels.Count > 0)
            currentChannel = radioChannels[channelIndex];
        
        /*
        ShuffleClips();
        ChangeChannel();
        */
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
            paused = true;
            audioSource.Pause();
        }
        else
        {
            paused = false;
            audioSource.UnPause();
        }
    }

    private void Update()
    {
        if (notfuckingreadyyet)
            return;
        if (!paused && !audioSource.isPlaying)
        {
            // next track
            if (song)
            {
                if (songCount == 0)
                {
                    // play a transition
                    audioSource.clip = currentChannel.trackToAdvert[Random.Range(0, currentChannel.trackToAdvert.Count)];
                    advertCount = Random.Range((int)advertRange[0], (int)advertRange[1] + 1);
                    song = false;
                }
                else
                {
                    songCount--;
                    audioSource.clip = currentChannel.NextTrack();
                }
            }
            else
            {
                if (advertCount == 0)
                {
                    // play transition
                    audioSource.clip = currentChannel.trackToAdvert[Random.Range(0, currentChannel.advertToTrack.Count)];
                    songCount = Random.Range((int)songRange[0], (int)songRange[1] + 1);
                    song = true;
                }
                else
                {
                    advertCount--;
                    audioSource.clip = advertChannel.NextTrack();
                }
            }
            audioSource.Play();
        }
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
        channelIndex++;
        if (channelIndex >= radioChannels.Count) 
        {
            channelIndex = 0;
        }
        currentChannel = radioChannels[channelIndex];
        //audioSource.clip = clips[clipIndex];
        //audioSource.time = Mathf.Min(clipTime % audioSource.clip.length, audioSource.clip.length - 0.01f);
        //if (!audioSource.isPlaying) audioSource.Play();
        coroutine = null;
    }


    /*
    private void ShuffleClips()
    {
        clips = new List<AudioClip>();
        musicClips.ForEach(clip => clips.Add(clip));
        advertClips.ForEach(clip => clips.Add(clip));
        clips.Shuffle();
    }
    
    private void Update()
    {
        clipTime += Time.deltaTime;
    }
    */
}
