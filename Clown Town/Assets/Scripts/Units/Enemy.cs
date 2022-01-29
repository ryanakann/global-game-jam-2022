using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().flipX = true;
    }
}
