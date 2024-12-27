using UnityEngine;
using System.Collections.Generic;

public class ItemPlacementChecker : MonoBehaviour
{
    // 在Inspector中指定三個需要放置的物品
    public List<GameObject> requiredItems; // 必須放置的物品列表
    private List<GameObject> placedItems = new List<GameObject>(); // 已經放置的物品

    void OnTriggerEnter(Collider other)
    {
        // 如果進入的物體是要求放置的物品，則將其加入 placedItems 列表
        if (requiredItems.Contains(other.gameObject) && !placedItems.Contains(other.gameObject))
        {
            placedItems.Add(other.gameObject);

            // 當所有物品都已經放置時，執行某個操作
            if (placedItems.Count == requiredItems.Count)
            {
                // 在這裡執行物品放置後的操作
                DoAction();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 如果物體離開區域，從 placedItems 列表中移除該物體
        if (placedItems.Contains(other.gameObject))
        {
            placedItems.Remove(other.gameObject);
        }
    }

    // 當所有物品都放置後要執行的操作
    void DoAction()
    {
        Debug.Log("所有物品已放置，執行操作！");
        // 你可以在這裡執行任何你需要的操作，例如解鎖一個物品、播放動畫等
    }
}
