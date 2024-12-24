using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RabbitTriggerPressOpenDetail : MonoBehaviour
{
    // 靜態變數，所有 TriggerPressOpenDetail 實例共享
    public static bool hasTriggered = false; // 防止重複執行的旗標

    public GameObject Canvas; // 要啟用的目標 Canvas
    public GameObject UpperDetail; // 要啟用的 UpperDetail
    public GameObject BottomDetail; // 要啟用的 BottomDetail
    public GameObject thisone;
    private XRRayInteractor rayInteractor;


    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage; // 負責更新圖片的腳本
    public LineRenderer lineRenderer; // 負責顯示線條的組件

    // 每幀更新檢查輸入
    void Update()
    {
        CheckTriggerAndEnableGameObject(thisone);
    }

    //private void CheckTriggerAndEnableGameObject(GameObject uipanel)
    //{
    //    InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

    //    if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
    //    {
    //        if (isTriggerPressed && !hasTriggered) // 第一次按下時觸發
    //        {
    //            ActivateGameObjects(uipanel);
    //            PrintObjectActivationStatus(uipanel);
    //            hasTriggered = true; // 防止重複觸發
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Failed to retrieve trigger button status.");
    //    }
    //}
    private void CheckTriggerAndEnableGameObject(GameObject uipanel)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            if (isTriggerPressed && !hasTriggered)
            {
                if (IsRaycastHitTarget(uipanel)) // 檢查是否擊中正確目標
                {
                    ActivateGameObjects(uipanel);
                    PrintObjectActivationStatus(uipanel);
                    hasTriggered = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Failed to retrieve trigger button status.");
        }
    }

    private bool IsRaycastHitTarget(GameObject target)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            return hit.collider.gameObject == target;
        }
        return false;
    }




    private void ActivateGameObjects(GameObject uipanel)
    {
        if (Canvas != null)
        {
            Canvas.SetActive(true);
            Debug.Log("[RabbitTriggerPressOpenDetail] Canvas enabled.");
        }

        if (UpperDetail != null)
        {
            UpperDetail.SetActive(true);
            Debug.Log($"[RabbitTriggerPressOpenDetail] {UpperDetail.name} has been enabled.");
        }

        if (BottomDetail != null)
        {
            EnableAllChildObjectsRecursive(BottomDetail);
            Debug.Log($"[RabbitTriggerPressOpenDetail] {BottomDetail.name} and all its children have been enabled.");
        }

        if (generateDetailUpperRecycleImage != null)
        {
            generateDetailUpperRecycleImage.UpdateImagesBasedOnCondition(uipanel.name);
            Debug.Log($"[RabbitTriggerPressOpenDetail] ImageSpawner updated with condition: {uipanel.name}.");
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    private void PrintObjectActivationStatus(GameObject bottomDetail)
    {
        Debug.Log($"[RabbitTriggerPressOpenDetail] Canvas is {(Canvas != null && Canvas.activeSelf ? "enabled" : "disabled or null")}");

        Debug.Log($"[RabbitTriggerPressOpenDetail] UpperDetail is {(UpperDetail != null && UpperDetail.activeSelf ? "enabled" : "disabled or null")}");

        if (BottomDetail != null && BottomDetail.activeSelf)
        {
            Debug.Log($"[RabbitTriggerPressOpenDetail] BottomDetail is enabled. Name: {BottomDetail.name}");
            PrintAllChildObjectNames(BottomDetail);
        }
        else
        {
            Debug.Log("[RabbitTriggerPressOpenDetail] BottomDetail is disabled or null.");
        }
    }

    private void PrintAllChildObjectNames(GameObject bottomDetail)
    {
        Debug.Log($"[RabbitTriggerPressOpenDetail] Parent: {bottomDetail.name}");

        foreach (Transform child in bottomDetail.transform)
        {
            Debug.Log($"[RabbitTriggerPressOpenDetail] Child: {child.gameObject.name}");
            PrintAllChildObjectNames(child.gameObject); // 遞迴列出子物件
        }
    }

    private void EnableAllChildObjectsRecursive(GameObject bottomDetail)
    {
        if (bottomDetail != null)
        {
            bottomDetail.SetActive(true);

            foreach (Transform child in bottomDetail.transform)
            {
                EnableAllChildObjectsRecursive(child.gameObject);
            }
        }
    }


    // 重置 hasTriggered 靜態標誌
    public static void ResetTrigger()
    {
        hasTriggered = false;
        Debug.Log("Trigger flag has been reset.");
    }
}
