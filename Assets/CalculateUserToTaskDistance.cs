using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CalculateUserToTaskDistance : MonoBehaviour
{
    public Transform frontPanelContainer;  // 前方 UI 容器
    public Transform backPanelContainer;   // 後方 UI 容器
    private float spacing = 400f; // UI 面板之間的間距
    public Sprite leftImage;
    public Sprite rightImage;
    public Sprite upImage;
    public Sprite downImage;
    void Update()
    {
        // 可根據需求調用 CalculateUserPositionToObject，傳入目標物件
    }

    public void UIPanelPosition(GameObject uiPanel, GameObject targetObject)
    {
        Vector3 objectPosition = targetObject.transform.position;
        Vector3 userPosition = GetUserPosition();

        // 判斷物件在使用者的前方或後方
        string positionRelation = GetFrontOrBack(userPosition, objectPosition);

        // 設定 UI 面板應該放置的區塊
        Transform parentContainer = positionRelation == "Front" ? frontPanelContainer : backPanelContainer;

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

    private string GetFrontOrBack(Vector3 userPosition, Vector3 objectPosition)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found. Returning 'Unknown'.");
            return "Unknown";
        }

        Vector3 userForward = mainCamera.transform.forward;
        Vector3 targetDirection = objectPosition - userPosition;

        // 根據 Dot 產品來判斷前後
        float dotForward = Vector3.Dot(userForward, targetDirection.normalized);
        return dotForward > 0 ? "Front" : "Back";
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
        string frontOrBack = GetFrontOrBack(userPosition, objectPosition);

        // 找到 TextMeshPro 文字元件
        TextMeshProUGUI textComponent = uiPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = $"{distance:F2}m";
            //textComponent.text = $"{distance:F2}m {positionRelation}";
        }

        // 找到 UI 面板內的 Image 物件
        Image leftRightImage = FindDeepChild(uiPanel.transform, "ImageLeftRight")?.GetComponent<Image>();
        Image upDownImage = FindDeepChild(uiPanel.transform, "ImageUpDown")?.GetComponent<Image>();

        // 設定左右的圖片
        if (leftRightImage != null)
        {
            leftRightImage.sprite = positionRelation == "Left" ? leftImage : rightImage;
        }

        // 設定上下的圖片
        if (upDownImage != null)
        {
            upDownImage.sprite = frontOrBack == "Front" ? upImage : downImage;
        }

        Debug.Log($"Target {targetObject.name} is {positionRelation} at {distance:F2}m.");
    }

    private Transform FindDeepChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
            Transform found = FindDeepChild(child, childName);
            if (found != null)
                return found;
        }
        return null;
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
