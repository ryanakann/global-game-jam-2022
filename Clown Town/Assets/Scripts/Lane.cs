using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane {
    public Vector2 start;
    public Vector2 end;
    protected List<GameObject> objects { get; }//indexed by grid position within lane
    public int GridIndex(Vector2 position) {
        return 0;
    }
}
