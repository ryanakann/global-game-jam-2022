using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FX_Button : MonoBehaviour
{
    public FXType buttonSound = FXType.MenuClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Click()
    {
        FX_Spawner.instance.SpawnFX(buttonSound, Vector3.zero, Quaternion.identity);
    }
}
