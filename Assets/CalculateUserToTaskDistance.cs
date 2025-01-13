using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateUserToTaskDistance : MonoBehaviour
{

    void Update()
    {
        // 可根據需求調用 CalculateUserPositionToObject，傳入目標物件
    }

    public void UIPanelPosition(GameObject uiPanel, GameObject targetObject) {

        Vector3 objectPosition = targetObject.transform.position;

        // 獲取使用者的位置
        Vector3 userPosition = GetUserPosition();

        // 判斷目標在使用者的左方、右方、前方或後方
        string positionRelation = GetPositionRelation(userPosition, objectPosition);

        // 根據目標位置的相對方向決定UI面板的位置
        // Store the initial position of the UI panel
        // 獲取 UI 面板的 RectTransform
        RectTransform uiPanelRectTransform = uiPanel.GetComponent<RectTransform>();

        if (positionRelation == "Right" || positionRelation == "Left") // 檢查是否要將面板放置在左邊或右邊
        {
            if (uiPanelRectTransform != null)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    // 計算相機的右方向
                    Vector3 rightDirection = mainCamera.transform.right;

                    // 計算相機的前方向
                    Vector3 forwardDirection = mainCamera.transform.forward;

                    // 預設一個初始位置
                    Vector3 targetPosition = mainCamera.transform.position;

                    //if (positionRelation == "Right")
                    //{
                    //    // 將 UI 面板放置在相機前方右邊
                    //    targetPosition = mainCamera.transform.position + forwardDirection * 5f + rightDirection * 2f; // 5f 是距離相機的前方，2f 是右邊的偏移量
                    //}
                    //else if (positionRelation == "Left")
                    //{
                    //    // 將 UI 面板放置在相機前方左邊
                    //    targetPosition = mainCamera.transform.position + forwardDirection * 5f - rightDirection * 1f; // 5f 是距離相機的前方，-2f 是左邊的偏移量
                    //}

                    // 添加一個 offsetIndex，確保每個面板有不同的偏移
                    int offsetIndex = uiPanel.transform.GetSiblingIndex(); // 或其他方式獲取唯一索引
                    float offset = offsetIndex * 2f; // 每個面板的間隔距離

                    if (positionRelation == "Right")
                    {
                        targetPosition = mainCamera.transform.position + forwardDirection * 5f + rightDirection * (2f + offset);
                    }
                    else if (positionRelation == "Left")
                    {
                        targetPosition = mainCamera.transform.position + forwardDirection * 5f - rightDirection * (1f + offset);
                    }




                    // 更新 UI 面板的位置
                    uiPanelRectTransform.position = targetPosition;

                    // 輸出新的世界座標
                    Debug.Log($"UI Panel moved to new position: {uiPanelRectTransform.position}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Unexpected positionRelation value: {positionRelation}");
        }





    }

    public void CalculateUserPositionToObject(GameObject targetObject, GameObject uiPanel)
    {
        Vector3 objectPosition = targetObject.transform.position;

        // 獲取使用者的位置
        Vector3 userPosition = GetUserPosition();

        // 計算使用者到目標的距離
        float distance = Vector3.Distance(userPosition, objectPosition);

        string positionRelation = GetPositionRelation(userPosition, objectPosition);

        // 查找場景中的 TMP Text 物件，名稱為 "targetObject+_distance"
        string textObjectName = $"{targetObject.name}_distance";

        GameObject textObject = GameObject.Find(textObjectName);
        if (textObject != null)
        {
            // 嘗試獲取 TextMeshPro (UI) 組件
            TextMeshProUGUI textUIComponent = textObject.GetComponent<TextMeshProUGUI>();
            if (textUIComponent != null)
            {
                // 更新文字內容為距離和位置關係
                textUIComponent.text = $"{distance:F2}m {positionRelation}";
            }
        }

        // 加入 log 訊息
        Debug.Log($"UpdatePanelPositionBasedOnUserDistance: Target {targetObject.name} is {positionRelation} at a distance of {distance:F2}m.");
    }

    private string GetPositionRelation(Vector3 userPosition, Vector3 objectPosition)
    {
        // 使用者的正前方向
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found. Returning 'Unknown' as fallback.");
            return "Unknown";
        }

        Vector3 userForward = mainCamera.transform.forward;
        Vector3 userRight = mainCamera.transform.right;

        // 從使用者到目標的方向向量
        Vector3 targetDirection = objectPosition - userPosition;

        // 判斷前後
        float dotForward = Vector3.Dot(userForward, targetDirection.normalized);
        bool isInFront = dotForward > 0;

        // 判斷左右
        float dotRight = Vector3.Dot(userRight, targetDirection.normalized);
        string leftOrRight = dotRight > 0 ? "Right" : "Left";

        return leftOrRight;
    }

    private Vector3 GetUserPosition()
    {
        // 假設使用 XR Rig，抓取主攝影機的位置（通常表示使用者位置）
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            return mainCamera.transform.position;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Returning Vector3.zero as fallback.");
            return Vector3.zero; // 回傳預設值
        }
    }
}
