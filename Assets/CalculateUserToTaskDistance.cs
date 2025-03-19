using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateUserToTaskDistance : MonoBehaviour
{
    public Transform leftPanelContainer;  // 左側 UI 容器
    public Transform rightPanelContainer; // 右側 UI 容器
    public float spacing = 1.5f; // UI 面板之間的間距

    void Update()
    {
        // 可根據需求調用 CalculateUserPositionToObject，傳入目標物件
    }

    public void UIPanelPosition(GameObject uiPanel, GameObject targetObject)
    {
        Vector3 objectPosition = targetObject.transform.position;
        Vector3 userPosition = GetUserPosition();

        // 判斷物件在使用者的左側或右側
        string positionRelation = GetLeftOrRight(userPosition, objectPosition);

        // 獲取 UI 面板的 RectTransform
        RectTransform uiPanelRectTransform = uiPanel.GetComponent<RectTransform>();

        if (uiPanelRectTransform != null)
        {
            // 設定 UI 面板應該放置的區塊
            Transform parentContainer = positionRelation == "Left" ? leftPanelContainer : rightPanelContainer;
            uiPanel.transform.SetParent(parentContainer, false);

            // 確保 UI 面板按照水平間距排列
            ArrangePanels(parentContainer);
        }
        else
        {
            Debug.LogWarning("UI Panel does not have a RectTransform.");
        }
    }

    private void ArrangePanels(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Transform panel = container.GetChild(i);
            panel.localPosition = new Vector3(i * spacing, 0, 0); // 水平方向排列
        }
    }

    private string GetLeftOrRight(Vector3 userPosition, Vector3 objectPosition)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found. Returning 'Unknown'.");
            return "Unknown";
        }

        Vector3 userRight = mainCamera.transform.right;
        Vector3 targetDirection = objectPosition - userPosition;

        // 根據 Dot 產品來判斷左右
        float dotRight = Vector3.Dot(userRight, targetDirection.normalized);
        return dotRight > 0 ? "Right" : "Left";
    }

    public void CalculateUserPositionToObject(GameObject targetObject, GameObject uiPanel)
    {
        Vector3 objectPosition = targetObject.transform.position;
        Vector3 userPosition = GetUserPosition();
        float distance = Vector3.Distance(userPosition, objectPosition);

        string positionRelation = GetLeftOrRight(userPosition, objectPosition);
        string textObjectName = $"{targetObject.name}_distance";

        GameObject textObject = GameObject.Find(textObjectName);
        if (textObject != null)
        {
            TextMeshProUGUI textUIComponent = textObject.GetComponent<TextMeshProUGUI>();
            if (textUIComponent != null)
            {
                textUIComponent.text = $"{distance:F2}m {positionRelation}";
            }
        }

        Debug.Log($"Target {targetObject.name} is {positionRelation} at {distance:F2}m.");
    }

    private Vector3 GetUserPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            return mainCamera.transform.position;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Returning Vector3.zero.");
            return Vector3.zero;
        }
    }
}
