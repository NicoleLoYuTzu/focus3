using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerPressOpenDetail : MonoBehaviour
{
    // 靜態變數，所有 TriggerPressOpenDetail 實例共享
    public static bool hasTriggered = false; // 防止重複執行的旗標

    public GameObject Canvas; // 要啟用的目標 Canvas
    public GameObject UpperDetail; // 要啟用的 UpperDetail
    public GameObject BottomDetail; // 要啟用的 BottomDetail

    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage; // 負責更新圖片的腳本
    public LineRenderer lineRenderer; // 負責顯示線條的組件

    // 每幀更新檢查輸入
    void Update()
    {
        CheckTriggerAndEnableGameObject();
    }

    private void CheckTriggerAndEnableGameObject()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed && !hasTriggered) // 第一次按下時觸發
            {
                ActivateGameObjects();
                PrintObjectActivationStatus();
                hasTriggered = true; // 防止重複觸發
            }
        }
        else
        {
            Debug.LogWarning("Failed to retrieve trigger button status.");
        }
    }

    private void ActivateGameObjects()
    {
        if (Canvas != null)
        {
            Canvas.SetActive(true);
            Debug.Log("Canvas enabled.");
        }

        if (UpperDetail != null)
        {
            UpperDetail.SetActive(true);
            Debug.Log($"{UpperDetail.name} has been enabled.");
        }

        if (BottomDetail != null)
        {
            EnableAllChildObjectsRecursive(BottomDetail);
            Debug.Log($"{BottomDetail.name} and all its children have been enabled.");
        }

        if (generateDetailUpperRecycleImage != null)
        {
            generateDetailUpperRecycleImage.UpdateImagesBasedOnCondition(gameObject.name);
            Debug.Log($"ImageSpawner updated with condition: {gameObject.name}.");
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    private void EnableAllChildObjectsRecursive(GameObject parent)
    {
        if (parent != null)
        {
            parent.SetActive(true);

            foreach (Transform child in parent.transform)
            {
                EnableAllChildObjectsRecursive(child.gameObject);
            }
        }
    }

    private void PrintObjectActivationStatus()
    {
        Debug.Log($"PrintObjectActivationStatus Canvas is {(Canvas != null && Canvas.activeSelf ? "enabled" : "disabled or null")}");

        Debug.Log($"PrintObjectActivationStatus UpperDetail is {(UpperDetail != null && UpperDetail.activeSelf ? "enabled" : "disabled or null")}");

        if (BottomDetail != null && BottomDetail.activeSelf)
        {
            Debug.Log($"PrintObjectActivationStatus BottomDetail is enabled. Name: {BottomDetail.name}");
            PrintAllChildObjectNames(BottomDetail);
        }
        else
        {
            Debug.Log("PrintObjectActivationStatus BottomDetail is disabled or null.");
        }
    }

    private void PrintAllChildObjectNames(GameObject parent)
    {
        Debug.Log($"PrintObjectActivationStatus Parent: {parent.name}");

        foreach (Transform child in parent.transform)
        {
            Debug.Log($"PrintObjectActivationStatus Child: {child.gameObject.name}");
            PrintAllChildObjectNames(child.gameObject); // 遞迴列出子物件
        }
    }

    // 重置 hasTriggered 靜態標誌
    public static void ResetTrigger()
    {
        hasTriggered = false;
        Debug.Log("Trigger flag has been reset.");
    }
}
