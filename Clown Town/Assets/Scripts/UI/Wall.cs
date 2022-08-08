using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Singleton<Wall>
{
    [HideInInspector]
    public bool moving;

    bool open = true;

    float switchTimer = 1.5f;

    [HideInInspector]
    public float progress;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = SelectionController.instance.mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 1f)) + new Vector3(0,0,10);
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        Vector2 screenSize = new Vector2(ScreenSize.GetScreenToWorldWidth, ScreenSize.GetScreenToWorldHeight);
        var ratio = new Vector3(screenSize.x / spriteSize.x, screenSize.y / spriteSize.y, 1);

        transform.localScale = ratio;
        //float width = ScreenSize.GetScreenToWorldWidth;
        //transform.localScale = Vector3.one * width;
    }

    public void Switch(bool _open)
    {
        if (open == _open)
            return;
        else
            open = _open;
        FX_Spawner.instance.SpawnFX(FXType.WallMove, Vector3.zero, Quaternion.identity);
        StartCoroutine(CoSwitch());
    }

    IEnumerator CoSwitch()
    {
        moving = true;
        Vector3 target;
        if (open)
            target = SelectionController.instance.mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 1f));
        else
            target = SelectionController.instance.mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0f));

        var timer = 0f;
        target += new Vector3(0f, 0f, 10f);
        Vector3 pos = transform.position;
        while (timer < switchTimer)
        {
            transform.position = Vector3.Lerp(pos, target, timer / switchTimer);
            progress = timer / switchTimer;
            timer += Time.deltaTime;
            yield return null;
        }
        moving = false;
    }
}
