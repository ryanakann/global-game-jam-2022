using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownsDisplay : Singleton<ClownsDisplay>
{
    Transform minWidthPivot, maxWidthPivot;

    Transform clownDisplayHolder;

    public void Generate()
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
            float clownX = minX + (clownSpacing * (i+0.5f));

            Vector2 pos = new Vector2(clownX, theY);
            var display = clown.SpawnDisplayAtPosition(pos);
            clown.display = display;
            display.transform.localScale = new Vector3(2, 2, 1);
            display.highlight.gameObject.SetActive(false);
            display.selectHighlight.gameObject.SetActive(false);
            display.transform.parent = clownDisplayHolder;

        }
    }

    static public Vector3 GetClownsDisplayPos()
    {
        return instance.clownDisplayHolder.position;
    }

    public void ResetDisplay()
    {
        foreach (Transform t in clownDisplayHolder)
        {
            if (t.GetComponent<ClownDisplay>() != null)
                Destroy(t.gameObject);
        }

        Generate();
    }

    // Start is called before the first frame update
    void Start()
    {
        clownDisplayHolder = transform.FindDeepChild("ClownDisplayHolder");
        minWidthPivot = transform.FindDeepChild("minWidthPivot");
        maxWidthPivot = transform.FindDeepChild("maxWidthPivot");

        Generate();
    }

}
