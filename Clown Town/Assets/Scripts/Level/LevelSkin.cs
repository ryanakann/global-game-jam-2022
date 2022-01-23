using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Skin", menuName = "ScriptableObjects/Level Skin")]
public class LevelSkin : ScriptableObject
{
    public Bounds playArea;
    public bool showDebugArea;
}
