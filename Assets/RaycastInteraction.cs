using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class RaycastInteractor : MonoBehaviour
{
    public GameObject canvas; // UI 主面板
    public GameObject upperDetail; // 詳細資訊區域
    public LayerMask raycastLayerMask; // Raycast 目標的圖層
    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage; // 負責更新 UI 內容的腳本

    [System.Serializable]
    public class TargetUIPair
    {
        public GameObject selectableObject; // 可選擇的物件
        public GameObject selectableObjectDetail; // 對應的詳細資訊 UI
    }

    private InputDevice leftHandDevice; // VR 左手裝置
    private RaycastHit hitInfo; // Raycast 擊中的物件資訊
    public List<TargetUIPair> targetUIPairs; // 可互動物件的列表
    private GameObject currentSelectedObject = null; // 追蹤當前選中的物件

    void Start()
    {
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Debug.Log("Start: Left hand device initialized");
    }

    void Update()
    {
        // 取得左手 Trigger 按鈕狀態
        if (leftHandDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
        {
            Debug.Log("Update: Trigger pressed");

            // 發射 Raycast 檢測
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, raycastLayerMask))
            {
                Debug.Log("Raycast hit object: " + hitInfo.collider.gameObject.name);

                // 找到與 Raycast 擊中物件相對應的 UI 配對
                TargetUIPair matchedPair = targetUIPairs.Find(pair => pair.selectableObject == hitInfo.collider.gameObject);

                if (matchedPair != null)
                {
                    // 允許點擊相同物件開啟 UI
                    if (currentSelectedObject != matchedPair.selectableObject || !canvas.activeSelf)
                    {
                        ShowSelectableObjectDetail(matchedPair);
                        generateDetailUpperRecycleImage.ClearExistingButtons();
                        generateDetailUpperRecycleImage.UpdateImagesBasedOnCondition(matchedPair.selectableObject.name);
                        currentSelectedObject = matchedPair.selectableObject;
                    }
                }
                else if (currentSelectedObject != null)
                {
                    // 如果點擊的不是 targetUIPairs 內的物件，則關閉 UI
                    DisableDetailObjects();
                }
            }
            else
            {
                Debug.Log("No object hit by raycast - Closing UI");
                DisableDetailObjects();
            }
        }
    }

    /// <summary>
    /// 顯示選中物件的詳細資訊 UI
    /// </summary>
    private void ShowSelectableObjectDetail(TargetUIPair selectedPair)
    {
        Debug.Log("Showing details for object: " + selectedPair.selectableObject.name);
        canvas.SetActive(true);
        upperDetail.SetActive(true);

        if (!selectedPair.selectableObjectDetail.activeSelf)
        {
            selectedPair.selectableObjectDetail.SetActive(true);
            EnableAllChildObjects(selectedPair.selectableObjectDetail.transform);
        }
    }

    /// <summary>
    /// 遞迴啟用所有子物件
    /// </summary>
    private void EnableAllChildObjects(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(true);
            EnableAllChildObjects(child);
        }
    }

    /// <summary>
    /// 關閉 UI 並重置選擇狀態
    /// </summary>
    private void DisableDetailObjects()
    {
        if (!canvas.activeSelf) return; // UI 已經關閉，無需執行

        Debug.Log("Disabling all detail objects");
        canvas.SetActive(false);
        upperDetail.SetActive(false);
        generateDetailUpperRecycleImage.ClearExistingButtons();

        // 關閉所有 selectableObjectDetail
        foreach (var pair in targetUIPairs)
        {
            if (pair.selectableObjectDetail.activeSelf)
            {
                pair.selectableObjectDetail.SetActive(false);
            }
        }

        currentSelectedObject = null; // 清除當前選中物件
    }
}
