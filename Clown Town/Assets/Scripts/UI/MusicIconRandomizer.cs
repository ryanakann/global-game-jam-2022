using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIconRandomizer : MonoBehaviour
{
    public List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
    }
}
