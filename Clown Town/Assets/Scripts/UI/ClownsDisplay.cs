using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownsDisplay : MonoBehaviour
{
    Transform minWidthPivot, maxWidthPivot;
    

    void Generate()
    {
        List<Clown> clownsList = ClownManager.GetClowns();

        int numClowns = clownsList.Count;

        float theY = minWidthPivot.position.y;
        float maxX = maxWidthPivot.position.x;
        float minX = minWidthPivot.position.x;
        float width = maxX - minX;

        float clownSpacing = width / numClowns;

        for (int i=0; i<numClowns; i++)
        {
            Clown clown = clownsList[i];
            float clownX = minX + (clownSpacing * (i+1));

            Vector2 pos = new Vector2(clownX, theY);
            clown.SpawnDisplayAtPosition(pos);

        }
    }

    // Start is called before the first frame update
    void Start()
    {

        minWidthPivot = transform.FindDeepChild("minWidthPivot");
        maxWidthPivot = transform.FindDeepChild("maxWidthPivot");

        Generate();
    }

}
