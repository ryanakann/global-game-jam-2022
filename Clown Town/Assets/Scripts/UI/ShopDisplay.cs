using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDisplay : MonoBehaviour {
    [SerializeField]
    ShopItemList items;
    [SerializeField]
    GameObject ShopItemUI;

    void Start() {
        foreach (ShopItem item in items.shopItems) {
            FillUIForShopItem(item);
        }
    }

    GameObject FillUIForShopItem(ShopItem item) {
        GameObject itemUI = Instantiate(ShopItemUI, transform.FindDeepChild("Content"));
        itemUI.GetComponent<Button>().onClick.AddListener(delegate {
            print("trying to buy a " + item.prefab.name);
            //TODO: attempt to buy the item
            //BuildController.instance.onClickShopButton(item);
        });
        itemUI.GetComponent<Image>().sprite = item.icon;
        itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.name.ToUpper();
        itemUI.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "" + item.cost;
        return itemUI;
    }
}
