using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {
    public Lane lane;

    void Start() {
        SetToPositionWithinLane(0);
    }

    void Update() {
        //project position onto line of lane
        //Vector2 l = lane.end - lane.start
        //transform.position = Vector2.Dot(lane.end - lane.start,transform.position-lane.start)
    }

    public void SetToPositionWithinLane(float t) {
        //lerp
    }
}
