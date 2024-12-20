using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerPressOpenDetail : MonoBehaviour
{
    public GameObject Canvas; // 要啟用的目標 GameObject
    public GameObject UpperDetail; // 要啟用的目標 GameObject
    public GameObject BottomDetail; // 要啟用的目標 GameObject

    private bool isTriggerPressedPreviously = false; // 記錄上一次的按鍵狀態
    private ImageSpawner imageSpawner;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TriggerPressOpenDetail Update called");
        CheckTriggerAndEnableGameObject();
    }

    private void CheckTriggerAndEnableGameObject()
    {
        Debug.Log("Checking trigger button status...");

        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // 檢測扳機按鈕是否按下
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed))
        {
            Debug.Log($"Trigger button status: {isTriggerPressed}, Previous status: {isTriggerPressedPreviously}");

            // 當按鍵從未按下變為按下的瞬間觸發
            if (isTriggerPressed && !isTriggerPressedPreviously)
            {
                Debug.Log("Trigger button pressed for the first time in this cycle.");

                if (UpperDetail != null)
                {
                    Debug.Log("UpperDetail is not null. Proceeding to enable UI elements.");

                    Canvas.SetActive(true);
                    Debug.Log("Canvas enabled.");

                    BottomDetail.SetActive(true);
                    Debug.Log("BottomDetail enabled.");

                    UpperDetail.SetActive(true);
                    Debug.Log($"{UpperDetail.name} has been enabled.");

                    // 更新圖片或其他狀態
                    imageSpawner.UpdateImagesBasedOnCondition(gameObject.name);
                    Debug.Log("ImageSpawner updated with condition: purpleGhost.");
                }
                else
                {
                    Debug.LogWarning("UpperDetail GameObject is not assigned!");
                }
            }
            else
            {
                Debug.Log("Trigger button not pressed or still being held.");
            }

            // 更新按鍵狀態
            isTriggerPressedPreviously = isTriggerPressed;
            Debug.Log($"Updated previous trigger status to: {isTriggerPressedPreviously}");
        }
        else
        {
            Debug.LogWarning("Failed to retrieve trigger button status.");
        }
    }

}
