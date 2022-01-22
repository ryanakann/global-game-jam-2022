using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lane {
    public Vector2 start;
    public Vector2 end;
    protected List<GameObject> objects { get; }//indexed by grid position within lane

    public GameObject obj;

    public Lane (int y, int laneCount, Rect playArea)
    {
        float laneLength = playArea.width;
        float laneWidth = playArea.height / laneCount;
        start = new Vector2(playArea.xMin, playArea.yMin + (0.5f + y) * laneWidth);
        end = start + Vector2.right * laneLength;

        obj = new GameObject($"Lane{y}");
        obj.transform.position = (start + end) / 2f;
        obj.transform.localScale = new Vector3(laneLength, 1f, 1f);
        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0f, 0f, 1f, 1f), Vector2.one / 2f, 1f);
        spriteRenderer.color = Color.white;
    }

    public int GridIndex(Vector2 position) {
        return 0;
    }

    public Vector2 Lerp(float t)
    {
        return Vector2.Lerp(start, end, t);
    }

    public float InverseLerp(Vector2 position)
    {
        Vector3 AB = end - start;
        Vector3 AV = position - start;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
