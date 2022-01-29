using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class SelectionController : Singleton<SelectionController>
{
    SelectionObject currentSelectionObject;


    // Update is called once per frame
    void Update()
    {
        if (currentSelectionObject != null)
        {
            if (currentSelectionObject.selectionState.canHighlight == false)
            {
                currentSelectionObject.Unhighlight();
                currentSelectionObject = null;
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(UtilsClass.GetMouseWorldPosition(), Vector3.forward);
        if (hit.collider != null)
        {
            var obj = hit.transform.GetComponent<SelectionObject>();
            if (obj != null && obj.selectionState.canHighlight == true)
            {
                if (obj != currentSelectionObject)
                {
                    if (currentSelectionObject != null)
                    {
                        currentSelectionObject.Unhighlight();
                    }
                    currentSelectionObject = obj;
                    obj.Highlight();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}
