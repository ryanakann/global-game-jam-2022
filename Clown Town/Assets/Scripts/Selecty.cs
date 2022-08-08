using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selecty : MonoBehaviour
{
    public void SELECTY()
    {
        if (SelectionController.instance.currentSelectionObject.GetType() == typeof(ClownDisplay))
        {
            FX_Spawner.instance.SpawnFX(FXType.ClownSelect, Vector3.zero, Quaternion.identity);
            ExplainerManager.Explain(Cue.ClownSelect);
        }
        else if (SelectionController.instance.currentSelectionObject.GetType() == typeof(Location))
        {
            FX_Spawner.instance.SpawnFX(FXType.LocationSelect, Vector3.zero, Quaternion.identity);
            ExplainerManager.Explain(Cue.LocationSelect);
        }
    }
}
