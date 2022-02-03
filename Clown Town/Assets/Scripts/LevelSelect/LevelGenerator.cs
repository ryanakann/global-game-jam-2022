using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGenerator : Singleton<LevelGenerator>
{
    public int depth, minWidth, maxWidth, maxEdges;

    public float wiggleRoom;

    Transform minDepthPivot, maxDepthPivot, minWidthPivot, maxWidthPivot;

    public GameObject locationPrefab, edgePrefab;

    public List<LocationType> locationTypes = new List<LocationType>();

    List<Location> locations = new List<Location>();
    // DetailsUI locationDetails, edgeDetails, defaultDetails;


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
    }

    public Location SpawnLocation(Vector2 location)
    {
        var node = Instantiate(locationPrefab, location, Quaternion.identity).GetComponent<Location>();
        node.SetLocationType(locationTypes[Random.Range(0, locationTypes.Count)]);
        locations.Add(node);
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
        foreach (var loc in lastLocations)
        {
            SpawnEdge(loc, end_node);
        }


        foreach (var loc in locations)
        {
            loc.Deactivate();
            loc.transform.position += new Vector3(0, 0, 1f);
            loc.difficulty = Mathf.Clamp(Mathf.CeilToInt(loc.outgoingConnections.Count / ((float)maxEdges) * 10), 1, 10);

            foreach (var edge in loc.ingoingConnections)
            {
                edge.fuelCost += Mathf.CeilToInt(loc.difficulty / 10.0f) * 3;
            }
        }

        start_node.difficulty = 1;
        end_node.difficulty = 10;

        start_node.Activate();
        start_node.Occupy();
        start_node.transform.position += new Vector3(0, 0, 1f);
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
