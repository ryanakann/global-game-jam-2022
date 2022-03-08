using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownFuntime : MonoBehaviour
{

    public int maxClownNum = 50;
    public PhysicsMaterial2D physMat;

    public bool BAD;

    List<GameObject> clowns = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("SpawnClown", 1f, 5f);

    }

    void SpawnClown()
    {
        if (clowns.Count > maxClownNum)
        {
            var clown = clowns[0];
            clowns.RemoveAt(0);
            Destroy(clown);
        }
        var go = Instantiate(ClownManager.instance.displayPrefab).GetComponent<ClownDisplay>();

        foreach (var r in go.GetComponentsInChildren<SpriteRenderer>())
        {
            r.maskInteraction = SpriteMaskInteraction.None;
        }

        go.SetHead(ClownManager.GetClownHead());
        go.SetBody(ClownManager.GetClownBody());
        go.gameObject.AddComponent<Rigidbody2D>();

        go.collisionEvent += go.Flip;

        Rigidbody2D rb = go.gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle * 50f);

        go.highlight.gameObject.SetActive(false);
        go.selectHighlight.gameObject.SetActive(false);
        foreach (var col in go.GetComponentsInChildren<BoxCollider2D>())
        {
            col.sharedMaterial = physMat;
        }

        if (BAD)
        {
            foreach (var r in go.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                r.color = new Color(1f, 0.2f, 0.2f);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
