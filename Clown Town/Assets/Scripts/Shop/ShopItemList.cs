using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Items", menuName = "ScriptableObjects/Shop Item List", order = 1)]
public class ShopItemList : ScriptableObject {
    public List<ShopItem> shopItems;
}
