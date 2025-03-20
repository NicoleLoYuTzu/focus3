using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculateUserToTaskDistance : MonoBehaviour
{
    public Transform leftPanelContainer;  // 左側 UI 容器
    public Transform rightPanelContainer; // 右側 UI 容器
    private float spacing = 400f; // UI 面板之間的間距

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

        // 設定 UI 面板應該放置的區塊
        Transform parentContainer = positionRelation == "Left" ? leftPanelContainer : rightPanelContainer;

        // 🚀 設置新的父物件
        uiPanel.transform.SetParent(parentContainer, false);


        // 排列 UI 面板
        ArrangePanels(parentContainer);
    }

    private void ArrangePanels(Transform container)
    {
        int panelCount = container.childCount;
        Debug.Log($"Container: {container.name}, Number of child objects: {panelCount}");

        if (panelCount == 0) return;

        float totalWidth = (panelCount - 1) * spacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < panelCount; i++)
        {
            Transform panel = container.GetChild(i);
            Vector3 newPosition = new Vector3(startX + (i * spacing), 0, 0);

            // **只有當位置確實變動時才更新**
            if (panel.localPosition != newPosition)
            {
                panel.localPosition = newPosition;
                Debug.Log($"UI Panel {panel.name} set position: {panel.localPosition}");
            }
            else
            {
                Debug.Log($"UI Panel {panel.name} position unchanged: {panel.localPosition}");
            }
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
