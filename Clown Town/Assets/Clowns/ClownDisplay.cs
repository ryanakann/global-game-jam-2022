using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownDisplay : SelectionObject
{

    [HideInInspector]
    Clown clown;

    ClownBody body;

    SpriteRenderer headRenderer;
    SpriteRenderer bodyRenderer;

    bool left = true;

    public GameEvent collisionEvent;

    Vector2 originalBodyOffset;

    public override void Awake()
    {
        base.Awake();
        headRenderer = transform.FindDeepChild("Head").GetComponent<SpriteRenderer>();
        bodyRenderer = transform.FindDeepChild("Body").GetComponent<SpriteRenderer>();
    }

    public void RemoveClown()
    {
        ClownsDisplay.instance.ResetDisplay();
    }
    
    public void SetClown(Clown clown)
    {
        this.clown = clown;
        clown.deathEvent += RemoveClown;
    }

    public void SetHead(Sprite headSprite)
    {
        if (headRenderer.GetComponent<BoxCollider2D>())
            Destroy(headRenderer.GetComponent<BoxCollider2D>());
        headRenderer.sprite = headSprite;
        headRenderer.gameObject.AddComponent<BoxCollider2D>();
    }

    public void SetBody(ClownBody clownBody)
    {
        body = clownBody;
        bodyRenderer.transform.position = new Vector3(
            bodyRenderer.transform.position.x + body.offset.x,
            bodyRenderer.transform.position.y + body.offset.y
        );
        originalBodyOffset = bodyRenderer.transform.position - transform.position;
        if (Random.Range(0, 2) == 1)
        {
            Left();
        }
        else
        {
            Right();
        }
        if (bodyRenderer.GetComponent<BoxCollider2D>())
            Destroy(bodyRenderer.GetComponent<BoxCollider2D>());
        bodyRenderer.gameObject.AddComponent<BoxCollider2D>();
    }

    public void Flip()
    {
        if (left)
        {
            Right();
        }
        else
        {
            Left();
        }
    }

    public void Left()
    {
        var offset = ((Vector2)transform.position + originalBodyOffset);
        bodyRenderer.transform.position = new Vector3(
            offset.x + body.offset.x,
            offset.y
        );
        bodyRenderer.sprite = body.leftSprite;
        headRenderer.flipX = true;
        left = true;
    }

    public void Right()
    {
        var offset = ((Vector2)transform.position + originalBodyOffset);
        bodyRenderer.transform.position = new Vector3(
            offset.x - body.offset.x,
            offset.y
        );
        bodyRenderer.sprite = body.rightSprite;
        left = false;
    }

    public override void FillDetailsPanel()
    {
        base.FillDetailsPanel();
        SelectionController.instance.clownPanel.FillText("ClownName", "Bozo");
        SelectionController.instance.clownPanel.FillText("ClownTraits", string.Join(", ", "Stinky", "Lazy"));
        SelectionController.instance.clownPanel.FillText("ClownHealth", $"Health: 0/10");
        SelectionController.instance.clownPanel.FillImage("ClownImage", headRenderer.sprite);
    }

    public override void Select()
    {
        base.Select();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.clownPanel, select: true))
        {
            FillDetailsPanel();
            SelectionController.instance.clownPanel.FillButton("ClownTalk", true);
            SelectionController.instance.clownPanel.FillButton("ClownDeselect", true);
        }
    }

    public override void Highlight()
    {
        base.Highlight();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.clownPanel, select: false))
        {
            FillDetailsPanel();
            SelectionController.instance.clownPanel.FillButton("ClownTalk", false);
            SelectionController.instance.clownPanel.FillButton("ClownDeselect", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionEvent?.Invoke();
    }
}
