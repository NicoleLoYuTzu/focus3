using UnityEngine;

public class PrefabHoverHandler : MonoBehaviour
{
    private Canvas activeCanvas; // 當前顯示的 Canvas
    private GameObject rabbitPreview;
    private GameObject caterpillarPreview;

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
        Transform rabbitPreviewTransform = canvasTransform.Find("RabbitPreview");

        if (rabbitPreviewTransform == null)
        {
            Debug.LogError("RabbitPreview not found under Canvas!");
            return; // 提前結束，因為 RabbitPreview 找不到
        }

        // 如果 RabbitPreview 存在，將其存入變量
        rabbitPreview = rabbitPreviewTransform.gameObject;
        Debug.Log("Successfully found RabbitPreview!");


        //找到毛毛蟲
        Transform caterpillarPreviewTransform = canvasTransform.Find("CaterpillarPreview");
        caterpillarPreview = caterpillarPreviewTransform.gameObject;
    }

    public void OnHoverEnter()
    {
        Debug.Log($"OnHoverEnter triggered for {gameObject.name}.");

        // 根據物件名稱切換對應的 UI 畫面
        switch (gameObject.name)
        {
            case "RABBIT1":
                if (rabbitPreview != null)
                {
                    Debug.Log("Activating RabbitPreview UI.");
                    rabbitPreview.SetActive(true); // 顯示 RabbitPreview
                    caterpillarPreview.SetActive(false);
                }
                else
                {
                    Debug.LogError("RabbitPreview is null. Cannot activate.");
                }
                break;

            case "CATERPILLER":
                Debug.Log("CATERPILLER hover detected. Implement logic here.");
                caterpillarPreview.SetActive(true); // 顯示 RabbitPreview
                rabbitPreview.SetActive(false); // 顯示 RabbitPreview
                break;

            default:
                Debug.LogWarning($"No UI mapped for {gameObject.name}.");
                break;
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
}
