using UnityEngine;
using UnityEngine.AI; // 引入导航命名空间
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.UIElements; // 引入UI命名空間

public class PrefabHoverHandler : MonoBehaviour
{
    private GameObject rabbit;
    private GameObject purpleGhost;
    private GameObject Caterpillar;

    void Start()
    {
        rabbit = GameObject.Find("RabbitPreview");
        purpleGhost = GameObject.Find("PurpleGhostPreview");
        Caterpillar = GameObject.Find("CaterpillarPreview");
    }

    public void OnHoverEnter()
    {
        Debug.Log($"OnHoverEnter triggered for {gameObject.name}.");

        // 根據物件名稱切換對應的 UI 畫面
        switch (gameObject.name)
        {
            case "purpleGhost":
                SetActiveRecursively(purpleGhost, true); // 顯示 RabbitPreview 及其所有子物件
                SetActiveRecursively(rabbit, false); // 顯示 RabbitPreview 及其所有子物件
                SetActiveRecursively(Caterpillar, false);
                break;
            case "Rabbit1":
                SetActiveRecursively(rabbit, true); // 顯示 RabbitPreview 及其所有子物件
                SetActiveRecursively(purpleGhost, false);
                SetActiveRecursively(Caterpillar, false);
                break;

            case "CATERPILLER":
                SetActiveRecursively(Caterpillar, true); // 顯示CaterpillarPreview 及其所有子物件
                SetActiveRecursively(rabbit, false); // 隱藏RabbitPreview 及其所有子物件
                SetActiveRecursively(purpleGhost, false);
                break;

            default:
                Debug.LogWarning($"No UI mapped for {gameObject.name}.");
                break;
        }
    }

    // 遞迴設置物件及其所有子物件的 Active 狀態
    private void SetActiveRecursively(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            Debug.Log($"SetActiveRecursively Setting {obj.name} to {(isActive ? "Active" : "Inactive")}");
            obj.SetActive(isActive);

            // 遞迴遍歷所有子物件，設置為相同的 Active 狀態
            foreach (Transform child in obj.transform)
            {
                SetActiveRecursively(child.gameObject, isActive);
            }
        }
        else
        {
            Debug.LogError("Trying to set active state on a null object.");
        }
    }


}

//void OnHoverExit()
//{
//    Debug.Log($"OnHoverExit triggered for {gameObject.name}.");

//    // 隱藏目前顯示的畫面（如果有需要）
//    if (rabbitPreview != null && rabbitPreview.activeSelf)
//    {
//        Debug.Log("Deactivating RabbitPreview UI.");
//        rabbitPreview.SetActive(false); // 隱藏 RabbitPreview
//    }
//}
