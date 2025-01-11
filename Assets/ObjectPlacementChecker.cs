using UnityEngine;
using System.Collections.Generic;

public class ItemPlacementChecker : MonoBehaviour
{
    // 在 Inspector 中指定三個需要放置的物品
    public List<GameObject> requiredItems; // 必須放置的物品列表
    private List<GameObject> placedItems = new List<GameObject>(); // 已經放置的物品

    // 動畫目標物件，需在 Inspector 中指定
    public GameObject animatedObject;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ItemPlacementChecker: OnTriggerEnter called. Checking if the object is a required item...");

        // 使用名稱來檢查是否為需要的物品
        if (requiredItems.Exists(item => item.name == other.gameObject.name) && !placedItems.Contains(other.gameObject))
        {
            Debug.Log("ItemPlacementChecker: Item " + other.gameObject.name + " added to placed items.");

            placedItems.Add(other.gameObject);

            // 當所有物品都已經放置時，執行某個操作
            if (placedItems.Count == requiredItems.Count)
            {
                Debug.Log("ItemPlacementChecker: All required items have been placed. Executing action!");
                DoAction();
            }
        }
        else
        {
            Debug.Log("ItemPlacementChecker: Item " + other.gameObject.name + " is either not a required item or already placed.");
        }
    }


    void OnTriggerExit(Collider other)
    {
        Debug.Log("ItemPlacementChecker: OnTriggerExit called. Checking if the object was in placed items...");

        // 使用名稱來檢查物體是否已經在 placedItems 中
        GameObject itemToRemove = placedItems.Find(item => item.name == other.gameObject.name);

        if (itemToRemove != null)
        {
            Debug.Log("ItemPlacementChecker: Item " + other.gameObject.name + " removed from placed items.");
            placedItems.Remove(itemToRemove);
        }
        else
        {
            Debug.Log("ItemPlacementChecker: Item " + other.gameObject.name + " was not in placed items.");
        }
    }


    // 當所有物品都放置後要執行的操作
    void DoAction()
    {
        Debug.Log("ItemPlacementChecker: DoAction called. Starting animation.");

        if (animatedObject != null)
        {
            Debug.Log("ItemPlacementChecker: Target object for animation found: " + animatedObject.name);

            // Ensure the target object has an Animation component
            Animation animation = animatedObject.GetComponent<Animation>();
            if (animation != null)
            {
                Debug.Log("ItemPlacementChecker: Animation component found on target object.");

                animation.enabled = true; // Enable the animation
                Debug.Log("ItemPlacementChecker: Animation component enabled.");
                // Play the default clip of the Animation
                animation.Play();
                Debug.Log("ItemPlacementChecker: Playing animation.");
            }
            else
            {
                Debug.LogError("ItemPlacementChecker: The target object is missing an Animation component!");
            }
        }
        else
        {
            Debug.LogError("ItemPlacementChecker: No target object specified for the animation!");
        }
    }
}
