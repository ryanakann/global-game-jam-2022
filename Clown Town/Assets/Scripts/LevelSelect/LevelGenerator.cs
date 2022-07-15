using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGenerator : Singleton<LevelGenerator>
{
    public int depth, minWidth, maxWidth, maxEdges;

    public float wiggleRoom;

    Transform minDepthPivot, maxDepthPivot, minWidthPivot, maxWidthPivot, pivotHolder;
    
    [HideInInspector]
    public Transform cameraPivot;

    public GameObject locationPrefab, edgePrefab;

    public List<LocationType> locationTypes = new List<LocationType>();

    List<Location> locations = new List<Location>();
    // DetailsUI locationDetails, edgeDetails, defaultDetails;

    float lerpTimeLimit = 0.5f;

    public static int maxDifficulty = 10;
    public static int minDifficulty = 1;

    float minEdgeDifficulty = 1;
    float maxEdgeDifficulty = 8;

    float minDistDifficulty = 0;
    float maxDistDifficulty = 6;

    [HideInInspector]
    public Transform startLocation;



    public void UpdateCamera(Vector3 pos)
    {
        StartCoroutine(CoUpdateCamera(new Vector3(pos.x, pos.y, cameraPivot.position.z)));
    }

    public IEnumerator CoUpdateCamera(Vector3 pos)
    {
        float timeLimit = 0;
        Vector3 originalPos = cameraPivot.position;
        while (timeLimit < lerpTimeLimit)
        {
            cameraPivot.position = Vector3.SlerpUnclamped(originalPos, pos, timeLimit/lerpTimeLimit);
            timeLimit += Time.deltaTime;
            yield return null;
        }
    }


    public void SpawnEdge(Location src, Location tgt)
    {
        var edge = Instantiate(edgePrefab, Vector2.Lerp(src.transform.position, tgt.transform.position, 0.5f), Quaternion.identity).GetComponent<Edge>();
        edge.edgeName = $"Road to {tgt.locationName}";
        edge.transform.up = (tgt.transform.position - src.transform.position).normalized;
        var renderer = edge.GetComponent<SpriteRenderer>();
        var cost = (tgt.transform.position - src.transform.position).magnitude - 0.1f;
        renderer.size = new Vector2(0.32f, cost);
        edge.fuelCost = Mathf.CeilToInt(cost);
        edge.highlight.size = new Vector2(edge.highlight.size.x, cost);
        edge.selectHighlight.size = new Vector2(edge.selectHighlight.size.x, cost);
        edge.src = src;
        edge.tgt = tgt;
        src.outgoingConnections.Add(edge);
        tgt.ingoingConnections.Add(edge);
        edge.transform.parent = pivotHolder;
    }

    public Location SpawnLocation(Vector2 location)
    {
        var node = Instantiate(locationPrefab, location, Quaternion.identity).GetComponent<Location>();
        node.SetLocationType(locationTypes[Random.Range(0, locationTypes.Count)]);
        locations.Add(node);
        node.transform.parent = pivotHolder;
        return node;
    }

    public void Generate()
    {
        Vector2 offset = new Vector2(minWidthPivot.position.x, minDepthPivot.position.y);
        List<Location> lastLocations = new List<Location>();
        List<Location> nextLocations = new List<Location>();

        // spawn start
        var start_node = SpawnLocation(minDepthPivot.position);

        lastLocations.Add(start_node);


        float depthIncrement = Mathf.Abs(maxDepthPivot.position.y - minDepthPivot.position.y) / depth;

        for (int i = 1; i < depth; i++)
        {
            int width = Random.Range(minWidth, maxWidth);
            float widthIncrement = Mathf.Abs(maxWidthPivot.position.x - minWidthPivot.position.x) / width;
            for (int j = 0; j < width; j++)
            {
                var node = SpawnLocation(new Vector2((j + 0.5f) * widthIncrement, i * depthIncrement) + (Random.insideUnitCircle * wiggleRoom) + offset);
                node.distance = i;
                nextLocations.Add(node);
            }

            foreach (var loc in lastLocations)
            {
                var edges = Random.Range(1, maxEdges);
                for (int k = 0; k < edges; k++)
                {
                    SpawnEdge(loc, nextLocations[Random.Range(0, nextLocations.Count)]);
                }
            }
            foreach (var loc in nextLocations)
            {
                if (loc.ingoingConnections.Count > 0)
                    continue;
                SpawnEdge(lastLocations[Random.Range(0, lastLocations.Count)], loc);
            }
            lastLocations = nextLocations;
            nextLocations = new List<Location>();
        }

        // spawn end
        var end_node = SpawnLocation(maxDepthPivot.position);
        end_node.finalLocation = true;
        foreach (var loc in lastLocations)
        {
            SpawnEdge(loc, end_node);
        }


        foreach (var loc in locations)
        {
            loc.Deactivate();
            loc.transform.position += new Vector3(0, 0, 1f);

            // distance points of difficulty, minDistDifficulty-maxDistDifficulty
            // optional location difficulty points, +locationDifficulty
            // random points? this will be slight, 
            // edge points, midEdgeDifficulty-maxEdgeDifficulty
            // wax cost is more expensive for harder locations
            float edgeRange = maxEdgeDifficulty - minEdgeDifficulty;
            loc.difficulty += (int)Mathf.Clamp(Mathf.CeilToInt(minEdgeDifficulty + (loc.outgoingConnections.Count / ((float)maxEdges) * edgeRange)), minEdgeDifficulty, maxEdgeDifficulty);
            float distRange = maxDistDifficulty - minDistDifficulty;
            loc.difficulty += (int)Mathf.Clamp(minDistDifficulty + (loc.distance / depth * distRange), minDistDifficulty, maxDistDifficulty);
            loc.difficulty += loc.locationType.difficultyMod;

            foreach (var edge in loc.ingoingConnections)
            {
                edge.fuelCost += Mathf.CeilToInt(loc.difficulty / 10.0f * 3);
            }
        }

        start_node.difficulty = 1;
        end_node.difficulty = 10;

        start_node.Activate();
        start_node.Occupy();
        start_node.transform.position += new Vector3(0, 0, 1f);
        startLocation = start_node.transform;
    }

    void Start()
    {
        minDepthPivot = transform.FindDeepChild("minDepthPivot");
        maxDepthPivot = transform.FindDeepChild("maxDepthPivot");
        minWidthPivot = transform.FindDeepChild("minWidthPivot");
        maxWidthPivot = transform.FindDeepChild("maxWidthPivot");
        pivotHolder = transform.FindDeepChild("PivotHolder");
        cameraPivot = Camera.main.transform;

        Generate();
        ExplainerManager.Explain(Cue.StartGame);
    }

    static public Vector3 GetStartLocationPos()
    {
        return instance.startLocation.position;
    }
}
