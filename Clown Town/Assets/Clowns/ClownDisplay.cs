using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownDisplay : MonoBehaviour
{

    ClownBody body;

    SpriteRenderer headRenderer;
    SpriteRenderer bodyRenderer;

    private void Awake()
    {
        headRenderer = transform.FindDeepChild("Head").GetComponent<SpriteRenderer>();
        bodyRenderer = transform.FindDeepChild("Body").GetComponent<SpriteRenderer>();
    }

    public void SetHead(Sprite headSprite)
    {
        headRenderer.sprite = headSprite;
    }

    public void SetBody(ClownBody clownBody)
    {
        body = clownBody;
        if (Random.Range(0, 2) == 1)
        {
            Left();
        }
        else
        {
            Right();
        }
    }

    public void Left()
    {
        bodyRenderer.transform.position = new Vector3(
            bodyRenderer.transform.position.x + body.offset.x,
            bodyRenderer.transform.position.y + body.offset.y
        );
        bodyRenderer.sprite = body.leftSprite;
        headRenderer.flipX = true;
    }

    public void Right()
    {
        bodyRenderer.transform.position = new Vector3(
            bodyRenderer.transform.position.x - body.offset.x,
            bodyRenderer.transform.position.y + body.offset.y
        );
        bodyRenderer.sprite = body.rightSprite;
    }
}
