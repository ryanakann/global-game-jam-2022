using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuipItem
{
    public Sprite face;
    public string speakerName;
    public string text;

    public QuipItem(Sprite face, string speakerName, string text)
    {
        this.face = face;
        this.speakerName = speakerName;
        this.text = text;
    }
}

public class QuipManager : Singleton<QuipManager>
{
    // get points
    // manage stack

    Transform p0;
    List<Transform> points = new List<Transform>();
    List<float> alphaPoints = new List<float>() { 1f, 0.75f, 0.5f};

    float timer, maxTimer = 0.5f;
    List<QuipItem> quipQueue = new List<QuipItem>();

    public GameObject bubblePrefab;

    public GameEvent QuipEvent;

    [HideInInspector]
    public int quipCount;

    // Start is called before the first frame update
    void Start()
    {
        p0 = transform.FindDeepChild("p0");
        points.Add(transform.FindDeepChild("p1"));
        points.Add(transform.FindDeepChild("p2"));
        points.Add(transform.FindDeepChild("p3"));
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (quipQueue.Count > 0 && timer <= 0)
        {
            // TODO: different clown voices and clown moods
            //FX_Spawner.instance.SpawnFX(FXType.Quip, Vector3.zero, Quaternion.identity);
            SpawnQuip(quipQueue[0]);
            quipQueue.RemoveAt(0);
        }
    }


    // TODO, add buffering to the queue?
    public void SpawnQuip(QuipItem item)
    {
        // fire event
        if (timer > 0)
        {
            quipQueue.Add(item);
            return;
        }
        FX_Spawner.instance.SpawnFX(FXType.Quip, Vector3.zero, Quaternion.identity);
        timer = maxTimer;
        var bubble = Instantiate(bubblePrefab).GetComponent<QuipBubble>();
        bubble.transform.parent = transform;
        bubble.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        bubble.GetComponent<RectTransform>().offsetMax = Vector2.one;
        bubble.Setup(item.face, item.speakerName, item.text);
        bubble.transform.position = p0.position;
        // instantiate bubble
        bubble.sequence = points;
        bubble.alphaSequence = alphaPoints;
        // give the bubble the sequence
        // give it event
        quipCount++;
        QuipEvent += bubble.UpdateBubble;
        QuipEvent?.Invoke();
    }
}
