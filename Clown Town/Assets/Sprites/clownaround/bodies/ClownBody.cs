using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CLOWN/ClownBody", fileName = "new ClownBody")]
public class ClownBody : ScriptableObject
{
    public Vector2 offset;

    public Sprite leftSprite;
    public Sprite rightSprite;
}
