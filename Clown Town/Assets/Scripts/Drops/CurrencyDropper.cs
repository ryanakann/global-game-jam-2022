using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDropper : MonoBehaviour
{
    public CurrencyDropperProfile profile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Drop();
        }
    }

    public void Drop()
    {
        List<CurrencyTuple> drops = profile.GetDrop();
        foreach (var drop in drops)
        {
            var target = DropManager.instance.FindDropTarget(drop.CurrencyDropType);
            if (target != null)
            {
                var dropObj = Instantiate(drop.drop).GetComponent<CurrencyDrop>();
                dropObj.transform.parent = DropManager.instance.canvas;
                dropObj.MoveToPoint(transform.position);
                dropObj.UpdateTarget(target);
            }
        }
    }
}
