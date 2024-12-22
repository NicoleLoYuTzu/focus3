using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerToCloseDetail : MonoBehaviour
{
    private bool isTriggerPressedPreviously = false; // 記錄上一次的按鍵狀態
    private TriggerPressOpenDetail[] openDetailScripts; // 用於存儲所有的 TriggerPressOpenDetail 腳本

    void Start()
    {
        // 獲取場景中所有的 TriggerPressOpenDetail 腳本
        openDetailScripts = FindObjectsOfType<TriggerPressOpenDetail>();
    }

    void Update()
    {
        CheckTriggerAndCloseGameObjects();
    }

    private void CheckTriggerAndCloseGameObjects()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // 檢測扳機按鈕是否按下
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            // 當按鍵從未按下變為按下的瞬間觸發
            if (isTriggerPressed && !isTriggerPressedPreviously)
            {
                Debug.Log("Trigger button pressed on controller. Checking for open UIs...");

                bool anyUIClosed = false;

                // 遍歷所有 TriggerPressOpenDetail 腳本，嘗試關閉對應的 UI
                foreach (var detailScript in openDetailScripts)
                {
                    if (detailScript.Canvas != null && detailScript.Canvas.activeSelf)
                    {
                        detailScript.Canvas.SetActive(false);
                        anyUIClosed = true;
                        Debug.Log($"Closed UI: {detailScript.Canvas.name}");
                    }

                    if (detailScript.UpperDetail != null && detailScript.UpperDetail.activeSelf)
                    {
                        detailScript.UpperDetail.SetActive(false);
                        Debug.Log($"Closed UpperDetail: {detailScript.UpperDetail.name}");
                    }

                    if (detailScript.BottomDetail != null && detailScript.BottomDetail.activeSelf)
                    {
                        DisableAllChildObjectsRecursive(detailScript.BottomDetail);
                        //detailScript.BottomDetail.SetActive(false);
                        Debug.Log($"Closed BottomDetail: {detailScript.BottomDetail.name}");
                    }
                }

                if (!anyUIClosed)
                {
                    Debug.Log("No open UIs found to close.");
                }
            }

            // 更新按鍵狀態
            isTriggerPressedPreviously = isTriggerPressed;
        }
        else
        {
            Debug.LogWarning("Failed to retrieve trigger button status.");
        }


    }

    private void DisableAllChildObjectsRecursive(GameObject parent)
    {
        if (parent != null)
        {
            // 遍歷子物件並遞迴關閉
            foreach (Transform child in parent.transform)
            {
                DisableAllChildObjectsRecursive(child.gameObject);
            }

            // 最後關閉自身
            parent.SetActive(false);
        }
    }


}
