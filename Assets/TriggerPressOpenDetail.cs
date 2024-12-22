using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerPressOpenDetail : MonoBehaviour
{
    public GameObject Canvas; // 要啟用的目標 GameObject
    public GameObject UpperDetail; // 要啟用的目標 GameObject
    public GameObject BottomDetail; // 要啟用的目標 GameObject

    public ImageSpawner imageSpawner;

    public LineRenderer lineRenderer;

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

            // 當按鍵從未按下變為按下的瞬間觸發
            if (isTriggerPressed)
            {
                Debug.Log("Trigger button pressed for the first time in this cycle.");

                    Debug.Log("UpperDetail is not null. Proceeding to enable UI elements.");

                    Canvas.SetActive(true);
                    Debug.Log("Canvas enabled.");

                    //BottomDetail.SetActive(true);
                    if (BottomDetail != null)
                    {
                        EnableAllChildObjectsRecursive(BottomDetail); // 啟用所有層級
                    }
                    Debug.Log("BottomDetail enabled.");

                    UpperDetail.SetActive(true);
                    Debug.Log($"{UpperDetail.name} has been enabled.");
                    Debug.Log($"ImageSpawner updated with condition: {gameObject.name}.");
                    lineRenderer.enabled = false;
                    // 更新圖片或其他狀態
                    
                   
                    imageSpawner.UpdateImagesBasedOnCondition(gameObject.name);
            }
            else
            {
                Debug.Log("Trigger button not pressed or still being held.");
            }
        }
        else
        {
            Debug.LogWarning("Failed to retrieve trigger button status.");
        }
    }

    private void EnableAllChildObjectsRecursive(GameObject parent)
    {
        if (parent != null)
        {
            parent.SetActive(true); // 啟用當前物件

            // 遍歷子物件
            foreach (Transform child in parent.transform)
            {
                EnableAllChildObjectsRecursive(child.gameObject); // 遞迴啟用子物件及其子孫
            }
        }
    }




}
