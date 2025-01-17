﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FX_SoundList : FX_Object
{
    public static Dictionary<string, int> soundListIndices;

    public List<AudioClip> clips = new List<AudioClip>();
    public bool randomize = true;
    AudioSource aud;
    int index;

    public int playCount = 1;
    public bool shuffle;

    // Start is called before the first frame update
    void Start() {
        aud = GetComponent<AudioSource>();
        if (!aud || clips.Count == 0) return;

        if (!randomize) {
            if (soundListIndices == null) {
                soundListIndices = new Dictionary<string, int>();
            }
            string objName = gameObject.name.Replace("(Clone)", "");
            if (soundListIndices.ContainsKey(objName)) {
                soundListIndices[objName] += 1;
                index = soundListIndices[objName];
            } else {
                soundListIndices.Add(objName, 0);
                index = 0;
            }
            //aud.clip = clips[index % clips.Count];
        } else {
            index = Random.Range(0, clips.Count);
            //aud.clip = clips[Random.Range(0, clips.Count)];
        }

        if (playCount > 1)
        {
            if (shuffle)
                clips.Shuffle();
            int count = 0;
            foreach (Transform child in transform)
            {
                if (child.name == "src")
                {
                    var child_aud = child.GetComponent<AudioSource>();
                    child_aud.volume = aud.volume;
                    child_aud.pitch = aud.pitch;
                    lifetime = Mathf.Max(lifetime, SetClip(child_aud, index));
                    index++;
                    count++;
                    if (count >= playCount)
                        break;
                }
            }
        }
        else
        {
            lifetime = Mathf.Max(lifetime, SetClip(aud, index));
        }

        Destroy(gameObject, lifetime);
    }

    float SetClip(AudioSource src, int index)
    {
        src.clip = clips[index % clips.Count];
        src.Play();

        if (vol != -1)
        {
            src.volume = vol;
        }

        src.pitch += Random.Range(-pitch_range, pitch_range);
        src.volume += Random.Range(-amp_range, 0);
        return src.clip.length;
    }
}