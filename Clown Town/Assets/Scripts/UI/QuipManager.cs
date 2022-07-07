using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuipManager : Singleton<QuipManager>
{
    // get points
    // manage stack

    Transform p0;
    List<Transform> points = new List<Transform>();
    List<float> alphaPoints = new List<float>() { 1f, 0.75f, 0.5f};

    public GameObject bubblePrefab;

    public GameEvent QuipEvent;

    // Start is called before the first frame update
    void Start()
    {
        p0 = transform.FindDeepChild("p0");
        points.Add(transform.FindDeepChild("p1"));
        points.Add(transform.FindDeepChild("p2"));
        points.Add(transform.FindDeepChild("p3"));
    }


    // TODO, add buffering to the queue?
    public void SpawnQuip(Sprite face, string name, string text)
    {
        // fire event
        var bubble = Instantiate(bubblePrefab).GetComponent<QuipBubble>();
        bubble.transform.parent = transform;
        bubble.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        bubble.GetComponent<RectTransform>().offsetMax = Vector2.one;
        bubble.Setup(face, name, text);
        bubble.transform.position = p0.position;
        // instantiate bubble
        bubble.sequence = points;
        bubble.alphaSequence = alphaPoints;
        // give the bubble the sequence
        // give it event
        QuipEvent += bubble.UpdateBubble;
        QuipEvent?.Invoke();
    }
}
