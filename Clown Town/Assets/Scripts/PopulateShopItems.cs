#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
public class PopulateShopItems : UnityEditor.AssetModificationProcessor {

    static string[] OnWillSaveAssets(string[] paths) {
        ShopItemList allItems = (ShopItemList)AssetDatabase.LoadAssetAtPath("Assets/All Shop Items.asset", typeof(ShopItemList));
        allItems.shopItems = new List<ShopItem>();

        string prefabsFolder = Path.Combine(Application.dataPath, "Prefabs", "Units");
        string[] prefabPaths = Directory.GetFiles(prefabsFolder, "*.prefab", SearchOption.AllDirectories);
        foreach (string path in prefabPaths) {
            string assetPath = "Assets" + path.Replace(Application.dataPath, "").Replace("\\", "/");
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
            allItems.shopItems.Add(ShopItemFor(prefab));
        }
        Debug.Log("populated shop items");
        return paths;
    }
    static ShopItem ShopItemFor(GameObject prefab) {
        return new ShopItem
        {
            name = prefab.name,
            cost = prefab.GetComponent<Unit>().cost,
            prefab = prefab,
            icon = prefab.GetComponent<SpriteRenderer>().sprite,
        };
    }
}
#endif