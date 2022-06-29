using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownDisplay : SelectionObject
{

    [HideInInspector]
    public Clown clown;

    ClownBody body;

    SpriteRenderer headRenderer;
    SpriteRenderer bodyRenderer;

    public GameEvent collisionEvent;

    Vector2 originalBodyOffset;

    Animator anim;

    bool flashing;

    int incomingDamage;
    public GameObject numberPrefab;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        headRenderer = transform.FindDeepChild("Head").GetComponent<SpriteRenderer>();
        bodyRenderer = transform.FindDeepChild("Body").GetComponent<SpriteRenderer>();
        foreach (var rend in GetComponentsInChildren<SpriteRenderer>())
        {
            rend.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            rend.sortingLayerName = "Mirror";
        }
    }

    public void Kill()
    {
        // add thing to head
        // deparent, launch
    }

    public void Harm(int damage, bool jump = true)
    {
        incomingDamage += damage;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Harm") || jump == false)
        {
            SpawnNumber();
        }
        else
        {
            anim.SetTrigger("Harm");
        }
        Flash();
    }

    public void Flash()
    {
        if (flashing)
            return;
        StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        flashing = true;
        float t = 0, max = 0.75f;
        while (t < max)
        {
            t += Time.deltaTime;
            foreach (var rend in GetComponentsInChildren<SpriteRenderer>())
            {
                rend.color = Color.Lerp(Color.white, Color.red, Mathf.Sin(t / max * Mathf.PI));
            }
            yield return null;
        }
        flashing = false;
    }

    public void SpawnNumber()
    {
        if (incomingDamage == 0 || incomingDamage > 99 || numberPrefab == null)
            return;

        // convert numbers to one or two
        // instantiate number and set numbers
        var num = Instantiate(numberPrefab).GetComponent<NumberEffect>();
        num.transform.position = transform.position;
        num.SetNumber(incomingDamage);
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
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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
    }

    public void Right()
    {
        var offset = ((Vector2)transform.position + originalBodyOffset);
        bodyRenderer.transform.position = new Vector3(
            offset.x - body.offset.x,
            offset.y
        );
        bodyRenderer.sprite = body.rightSprite;
    }

    public override void FillDetailsPanel()
    {
        base.FillDetailsPanel();
        SelectionController.instance.clownPanel.FillText("ClownName", clown.Name);
        SelectionController.instance.clownPanel.FillText("ClownTraits", clown.Personality.ToString());
        SelectionController.instance.clownPanel.FillText("ClownHealth", $"Health: {clown.CurrentHealth} outta 10");
        SelectionController.instance.clownPanel.FillImage("ClownImage", headRenderer.sprite, true);
    }

    public override void Select()
    {
        base.Select();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.clownPanel, select: true))
        {
            FillDetailsPanel();
            SelectionController.instance.clownPanel.FillButton("TalkOpen", true);
        }
    }

    public override void Highlight()
    {
        base.Highlight();
        if (SelectionController.instance.ActivatePanel(SelectionController.instance.clownPanel, select: false))
        {
            FillDetailsPanel();
            SelectionController.instance.clownPanel.FillButton("TalkOpen", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionEvent?.Invoke();
    }
}
