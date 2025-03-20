using UnityEngine;

public class PrefabHoverHandler : MonoBehaviour
{
    private GameObject rabbit;
    private GameObject purpleGhost;
    private GameObject Caterpillar;

    void Start()
    {
        Debug.Log("PrefabHoverHandler Start method called.");

        // 嘗試找到 Canvas 並從中查找 RabbitPreview
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            Debug.LogError("Canvas not found in the scene!");
            return; // 提前結束，因為 Canvas 找不到
        }

        Debug.Log("Canvas found. Searching for RabbitPreview...");

        // 如果 Canvas 存在，嘗試找到 RabbitPreview
        Transform canvasTransform = canvasObject.transform;
        //找到兔子
        Transform rabbitTransform = canvasTransform.Find("RabbitPreview");

        if (rabbitTransform == null)
        {
            Debug.LogError("RabbitPreview not found under Canvas!");
            return; // 提前結束，因為 RabbitPreview 找不到
        }

        // 如果 RabbitPreview 存在，將其存入變量
        rabbit = rabbitTransform.gameObject;
        Debug.Log("Successfully found RabbitPreview!");

        Transform PurpleGhostTransform = canvasTransform.Find("PurpleGhostPreview");
        purpleGhost = PurpleGhostTransform.gameObject;

        Transform CaterpillarTransform = canvasTransform.Find("CaterpillarPreview");
        Caterpillar = CaterpillarTransform.gameObject;

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
                if (rabbit != null && purpleGhost != null)
                {
                    Debug.Log("Activating CaterpillarPreview UI.");
                    SetActiveRecursively(Caterpillar, true); // 顯示CaterpillarPreview 及其所有子物件
                    SetActiveRecursively(rabbit, false); // 隱藏RabbitPreview 及其所有子物件
                    SetActiveRecursively(purpleGhost, false);
                }
                else
                {
                    Debug.LogError("RabbitPreview or CaterpillarPreview is null. Cannot activate.");
                }
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
