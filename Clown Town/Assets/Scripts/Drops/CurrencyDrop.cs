using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetTuple
{
    public Vector3 pos;
    public float duration;

    public TargetTuple(Vector3 pos, float duration)
    {
        this.pos = pos;
        this.duration = duration;
    }
}

public enum CurrencyDropType { Null, Peanut, Wax };
[RequireComponent(typeof(RectTransform))]
public class CurrencyDrop : MonoBehaviour
{

    List<TargetTuple> targetList = new List<TargetTuple>();

    Vector3 target;

    float progress;
    Vector3 startPos;
    public CurrencyDropType currencyDropType;
    private RectTransform rectTransform;
    private Vector2 uiOffset;

    public AnimationCurve scaleCurve;
    public AnimationCurve splooshCurve;
    Vector3 startScale;
    float rotationSpeed;
    float startRotation;
    float size = 0.025f;

    // Update is called once per frame
    void Update()
    {
        // if target not none, lerp
        // if reach target, bloop
        if (targetList.Count > 0)
        {
            progress += Time.deltaTime;

            //MoveToPoint(Vector3.Lerp(startPos, target.position, progress));
            if (targetList.Count > 1)
                transform.position = Vector3.Lerp(startPos, targetList[0].pos, splooshCurve.Evaluate(progress / targetList[0].duration));
            else
                transform.position = Vector3.Lerp(startPos, targetList[0].pos, progress / targetList[0].duration);

            transform.Rotate( Vector3.forward * rotationSpeed * Time.deltaTime);

            if (targetList.Count == 1)
            {
                transform.localScale = startScale * scaleCurve.Evaluate(progress);

                if (progress >= targetList[0].duration)
                {
                    Bloop();
                    Destroy(gameObject);
                }
            }
            else if (progress >= targetList[0].duration)
            {
                progress = 0;
                targetList.RemoveAt(0);
                startPos = transform.position;
                target = targetList[0].pos;
            }
        }
    }

    public virtual void Bloop()
    {
        SelectionController.instance.AddCurrency(currencyDropType);
    }

    public void UpdateTarget(RectTransform target)
    {
        transform.Rotate(Vector3.forward * Random.Range(0, 360f));
        transform.localScale *= Random.Range(0.5f, 1.5f);
        progress = 0;
        this.target = target.position;

        startPos = transform.position;
        startScale = transform.localScale;
        startRotation = transform.rotation.eulerAngles.z;
        rotationSpeed = Random.Range(-200f, 200f);

        targetList.Add(new TargetTuple(transform.position + (Vector3)Random.insideUnitCircle * 50f, Random.Range(0.05f, 0.2f)));
        targetList.Add(new TargetTuple(target.position, 1f));
    }

    /// <summary>
    /// $$anonymous$$ove the UI element to the world position
    /// </summary>
    /// <param name="objectTransformPosition"></param>
    public void MoveToPoint(Vector3 objectTransformPosition)
    {
        this.rectTransform = GetComponent<RectTransform>();
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(objectTransformPosition);  //convert game object position to VievportPoint

        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        rectTransform.anchorMin = viewportPoint - Vector2.one * size;
        rectTransform.anchorMax = viewportPoint + Vector2.one * size;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        

        /*
        // Get the rect transform
        this.rectTransform = GetComponent<RectTransform>();

        // Calculate the screen offset
        this.uiOffset = new Vector2(DropManager.instance.canvas.sizeDelta.x / 2f, DropManager.instance.canvas.sizeDelta.y / 2f);

        // Get the position on the canvas
        Vector2 viewportPoint = DropManager.instance.cam.WorldToViewportPoint(objectTransformPosition);
        print(viewportPoint);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;
        
        Vector2 proportionalPosition = new Vector2(viewportPoint.x * DropManager.instance.canvas.sizeDelta.x, viewportPoint.y * DropManager.instance.canvas.sizeDelta.y);

        // Set the position and remove the screen offset
        this.rectTransform.localPosition = proportionalPosition - uiOffset;
        */
    }

}
