using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PurpleGhostTriggerPressOpenDetail : MonoBehaviour
{
    public static bool hasTriggered = false;
    public GameObject Canvas;
    public GameObject UpperDetail;
    public GameObject BottomDetail;
    public GameObject thisone;
    private XRRayInteractor rayInteractor;

    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage;
    public LineRenderer lineRenderer;

    void Update()
    {
        CheckTriggerAndEnableGameObject(thisone);
    }

    //private void CheckTriggerAndEnableGameObject(GameObject uipanel)
    //{
    //    InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

    //    if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
    //    {
    //        if (isTriggerPressed && !hasTriggered)
    //        {
    //            ActivateGameObjects(uipanel);
    //            PrintObjectActivationStatus(uipanel);
    //            hasTriggered = true;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("[PurpleGhost] Failed to retrieve trigger button status.");
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
            Debug.Log("[PurpleGhost] Canvas enabled.");
        }

        if (UpperDetail != null)
        {
            UpperDetail.SetActive(true);
            Debug.Log($"[PurpleGhost] {UpperDetail.name} has been enabled.");
        }

        if (BottomDetail != null)
        {
            EnableAllChildObjectsRecursive(BottomDetail);
            Debug.Log($"[PurpleGhost] {BottomDetail.name} and all its children have been enabled.");
        }

        if (generateDetailUpperRecycleImage != null)
        {
            generateDetailUpperRecycleImage.UpdateImagesBasedOnCondition(uipanel.name);
            Debug.Log($"[PurpleGhost] ImageSpawner updated with condition: {uipanel.name}.");
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
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

    private void PrintObjectActivationStatus(GameObject bottomDetail)
    {
        Debug.Log($"[PurpleGhost] Canvas is {(Canvas != null && Canvas.activeSelf ? "enabled" : "disabled or null")}");
        Debug.Log($"[PurpleGhost] UpperDetail is {(UpperDetail != null && UpperDetail.activeSelf ? "enabled" : "disabled or null")}");

        if (BottomDetail != null && BottomDetail.activeSelf)
        {
            Debug.Log($"[PurpleGhost] BottomDetail is enabled. Name: {BottomDetail.name}");
            PrintAllChildObjectNames(BottomDetail);
        }
        else
        {
            Debug.Log("[PurpleGhost] BottomDetail is disabled or null.");
        }
    }

    private void PrintAllChildObjectNames(GameObject bottomDetail)
    {
        Debug.Log($"[PurpleGhost] Parent: {bottomDetail.name}");

        foreach (Transform child in bottomDetail.transform)
        {
            Debug.Log($"[PurpleGhost] Child: {child.gameObject.name}");
            PrintAllChildObjectNames(child.gameObject);
        }
    }

    public static void ResetTrigger()
    {
        hasTriggered = false;
        Debug.Log("[PurpleGhost] Trigger flag has been reset.");
    }
}
