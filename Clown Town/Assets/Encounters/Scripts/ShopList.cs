using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Shop List", fileName = "New Shop List")]
    public class ShopList : ScriptableObject
    {
        public List<ShopItem> items;
    }

    [System.Serializable]
    public class ShopItem
    {
        public GameObject item;
        [Range(0f, 100f)]
        public float price;
    }
}