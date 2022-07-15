using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicoManager : Singleton<MusicoManager>
{
    int musicLevel, playerLevel, interval;
    float shiftTime;
    Transform musicHolder;

    // Start is called before the first frame update
    void Start()
    {
        musicLevel = -2;
        playerLevel = 1;
        musicHolder = transform.FindDeepChild("MusicHolder");
        interval = Mathf.RoundToInt((transform.FindDeepChild("minDepthPivot").position.y - musicHolder.position.y) / (playerLevel - musicLevel));
        foreach (var t in gameObject.FindComponentsInChildrenWithTag<Transform>("MusicIcon"))
        {
            t.transform.localScale *= Random.Range(0, 2) * 2 - 1;
            t.GetComponentInChildren<Animator>().speed = Random.Range(0.5f, 2f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateMusic();
        }
    }

    static public Vector3 GetPos()
    {
        return instance.musicHolder.position;
    }

    public void AdvancePlayer()
    {
        FX_Spawner.instance.SpawnFX(FXType.MusicAdvance, Vector3.zero, Quaternion.identity);
        playerLevel++;
    }

    public void UpdateMusic()
    {
        // push the music transform forward
        StartCoroutine(ShiftMusic());
    }

    IEnumerator ShiftMusic()
    {
        float progress = 0;
        Vector3 startPos = musicHolder.position;
        Vector3 endPos = startPos + Vector3.up * interval;
        while (progress < 1f)
        {
            musicHolder.position = Vector3.Lerp(startPos, endPos, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        musicLevel++;
        if (musicLevel == playerLevel)
        {
            FaderCanvas.instance.GoAway("Gameover");
        }
    }
}
