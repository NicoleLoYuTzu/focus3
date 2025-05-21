using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerAnchorToShowDetail : MonoBehaviour
{
    public GameObject canvas; // UI 主面板
    public GameObject upperDetail; // 詳細資訊區域
    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage; // 負責更新 UI 內容的腳本
    [System.Serializable]
    public class TargetPair
    {
        public GameObject targetAnchor;         // 有 Box Collider 並設為 Trigger 的物件
        public GameObject targetPreview;  // 要顯示的 UI 預覽
    }

    public List<TargetPair> targetPairs = new List<TargetPair>();

    private InputDevice leftHandDevice;
    private GameObject currentTargetInRange = null;
    private GameObject currentPreviewShown = null;
    private bool lastButtonState = false;
    private bool isDetailOpen = false;

    void Start()
    {
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Debug.Log("Start: Left hand device initialized");
    }

    void Update()
    {
        if (currentTargetInRange == null) return;

        if (leftHandDevice.IsPressed(InputHelpers.Button.PrimaryButton, out bool isPressed))
        {
            if (isPressed && !lastButtonState)
            {
                // 切換模式：若已開啟則關閉，否則依照 currentTargetInRange 開啟
                if (isDetailOpen)
                {
                    DisableDetailObjects();
                    currentPreviewShown = null;
                    isDetailOpen = false;
                }
                else if (currentTargetInRange != null)
                {
                    // 找到對應的 TargetPair
                    TargetPair pair = targetPairs.Find(p => p.targetAnchor == currentTargetInRange);
                    if (pair != null)
                    {
                        ShowSelectableObjectDetail(pair);
                        generateDetailUpperRecycleImage.ClearExistingButtons();
                        generateDetailUpperRecycleImage.SingleHint(pair.targetAnchor.name);
                        currentPreviewShown = pair.targetPreview;
                        isDetailOpen = true;
                    }
                }
            }
            lastButtonState = isPressed;
        }
    }

    private void DisableDetailObjects()
    {
        Debug.Log("Disabling all detail objects");

        canvas.SetActive(false);
        upperDetail.SetActive(false);
        generateDetailUpperRecycleImage.ClearExistingButtons();

        foreach (var pair in targetPairs)
        {
            pair.targetPreview.SetActive(false);
        }

        isDetailOpen = false;
    }


    private void ShowSelectableObjectDetail(TargetPair selectedPair)
    {
        Debug.Log("Showing details for object: " + selectedPair.targetPreview.name);
        canvas.SetActive(true);
        upperDetail.SetActive(true);

        if (!selectedPair.targetPreview.activeSelf)
        {
            selectedPair.targetPreview.SetActive(true);
            EnableAllChildObjects(selectedPair.targetPreview.transform);
        }
    }

    private void EnableAllChildObjects(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(true);
            EnableAllChildObjects(child);
        }
    }



    public void SetCurrentTargetInRange(GameObject target)
    {
        currentTargetInRange = target;
    }

    public void ClearCurrentTarget()
    {
        currentTargetInRange = null;

        if (currentPreviewShown != null)
        {
            currentPreviewShown.SetActive(false);
            currentPreviewShown = null;
        }
        DisableDetailObjects();
    }
}
