using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int depth, width, maxEdges;

    public float wiggleRoom;

    Transform minDepthPivot, maxDepthPivot, minWidthPivot, maxWidthPivot;

    List<Location> locations;


    public void Generate()
    {
        Vector2 offset = new Vector2(minWidthPivot.position.x, minDepthPivot.position.y);

        // spawn start

        float depthIncrement = Mathf.Abs(maxDepthPivot.position.y - minDepthPivot.position.y) / depth;

        for (int i = 1; i < depth; i++)
        {
            float widthIncrement = Mathf.Abs(maxWidthPivot.position.x - minWidthPivot.position.x) / width;
            for (int j = 0; j < width; j++)
            {
                var go = new GameObject($"Depth{i}");
                go.transform.position = new Vector2((j+0.5f) * widthIncrement, i * depthIncrement) + (Random.insideUnitCircle * wiggleRoom) + offset;
            }
        }

        // spawn end
    }

    void Start()
    {
        minDepthPivot = transform.FindDeepChild("minDepthPivot");
        maxDepthPivot = transform.FindDeepChild("maxDepthPivot");
        minWidthPivot = transform.FindDeepChild("minWidthPivot");
        maxWidthPivot = transform.FindDeepChild("maxWidthPivot");

        Generate();
    }
}
