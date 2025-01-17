﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricAudio : MonoBehaviour
{
    public GameObject aud;
    List<Collider> bounds;

    public bool occlude;
    public float occlude_dist;

    // Start is called before the first frame update
    void Start()
    {
        if (!aud) aud = transform.FindDeepChild("Audio").gameObject;
        bounds = new List<Collider>(GetComponentsInChildren<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!aud) return;
        aud.SetActive(true);
    }

    Vector3 GetClosestPoint(Vector3 point)
    {
        float best_dist = float.PositiveInfinity;
        Vector3 best_loc = Vector3.zero;
        foreach (Collider c in bounds)
        {
            Vector3 loc = c.ClosestPoint(point);
            float dist = Vector3.Distance(loc, point);
            if (dist < best_dist)
            {
                best_dist = dist;
                best_loc = loc;
            }
        }
        return best_loc;
    }
}