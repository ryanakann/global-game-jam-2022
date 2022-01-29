using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShopVisibility : MonoBehaviour {
    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    [Range(0f, 1f)]
    private float damping = 0.2f;

    private Vector2 positionSmoothing;
    private bool open;
    private bool locked;
    private Vector2 target;

    private void Start() {
        open = false;
        locked = false;
        target = Vector2.zero;
        Toggle();
    }

    public void Toggle() {
        if (locked) return;
        open = !open;
        target = open ? Vector2.right * 128 : Vector2.zero;
        transform.Find("Arrow").GetComponent<RectTransform>().localScale *= new Vector2(-1, 1);
    }

    public void Close() {
        if (open) {
            //FX_Spawner.instance.SpawnFX(FXType.CloseShop, transform.position, Quaternion.identity);
            Toggle();
        }
    }

    public void Open() {
        if (!open) {
            //FX_Spawner.instance.SpawnFX(FXType.OpenShop, transform.position, Quaternion.identity);
            Toggle();
        }
    }

    public void Lock() {
        Close();
        locked = true;
    }

    public void Unlock() {
        locked = false;
        Open();
    }

    private void Update() {
        SetPositionOfPivot(Vector2.SmoothDamp(rectTransform.anchoredPosition, target, ref positionSmoothing, damping));
    }

    private void SetPositionOfPivot(Vector2 newPos) {
        rectTransform.anchoredPosition = newPos;
    }
}
