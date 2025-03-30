using UnityEngine;
using System.Collections.Generic;

public class ItemPlacementZone : MonoBehaviour
{
    [Header("必須放置的物品")]
    public List<GameObject> requiredItems = new List<GameObject>(); // 需要放置的物品
    private List<GameObject> placedItems = new List<GameObject>(); // 已放置的物品

    public RabbitGameManager gameManager; // 連結到 RabbitGameManager

    private void OnTriggerEnter(Collider other)
    {
        if (requiredItems.Contains(other.gameObject) && !placedItems.Contains(other.gameObject))
        {
            placedItems.Add(other.gameObject);
            Debug.Log($"ItemPlacementZone: {other.gameObject.name} 正確放置！");

            // 檢查是否所有物品都已放置
            if (placedItems.Count == requiredItems.Count)
            {
                Debug.Log("ItemPlacementZone: 所有物品都已放置！");
                gameManager.OnAllItemsPlaced(); // 通知遊戲管理器
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (placedItems.Contains(other.gameObject))
        //{
        //    placedItems.Remove(other.gameObject);
        //    Debug.Log($"ItemPlacementZone: {other.gameObject.name} 已移除！");
        //}
    }

    // 返回未放置的物品
    public List<GameObject> GetMissingItems()
    {
        List<GameObject> missingItems = new List<GameObject>();
        foreach (var item in requiredItems)
        {
            if (!placedItems.Contains(item))
            {
                missingItems.Add(item);
            }
        }
        return missingItems;
    }
}
