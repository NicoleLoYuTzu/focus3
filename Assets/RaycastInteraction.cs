using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class RaycastInteractor : MonoBehaviour
{
    public GameObject canvas;
    public GameObject upperDetail;
    public LayerMask raycastLayerMask;
    public GenerateDetailUpperRecycleImage generateDetailUpperRecycleImage;

    [System.Serializable]
    public class TargetUIPair
    {
        public GameObject selectableObject;
        public GameObject selectableObjectDetail;
    }

    private InputDevice leftHandDevice;
    private RaycastHit hitInfo;

    public List<TargetUIPair> targetUIPairs;

    private bool isAnyObjectSelected = false;
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

            // 進行 Raycast 檢測
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, raycastLayerMask))
            {
                Debug.Log("Raycast hit object: " + hitInfo.collider.gameObject.name);

                bool foundSelectable = false;

                foreach (var pair in targetUIPairs)
                {
                    if (pair.selectableObject == hitInfo.collider.gameObject)
                    {
                        if (currentSelectedObject != pair.selectableObject) // 只在選中新物件時執行
                        {
                            ShowSelectableObjectDetail(pair);
                            generateDetailUpperRecycleImage.ClearExistingButtons();
                            generateDetailUpperRecycleImage.UpdateImagesBasedOnCondition(pair.selectableObject.name);
                            currentSelectedObject = pair.selectableObject; // 更新目前選中的物件
                        }

                        foundSelectable = true;
                        isAnyObjectSelected = true;
                        break;
                    }
                }

//                當 Raycast 打到的物件不是 targetUIPairs 裡的任何 selectableObject
//並且之前有選過東西（isAnyObjectSelected == true）
//=> 這時候關閉 UI
                if (!foundSelectable && isAnyObjectSelected)
                {
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

    private void EnableAllChildObjects(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(true);
            EnableAllChildObjects(child);
        }
    }

    private void DisableDetailObjects()
    {
        Debug.Log("Disabling all detail objects");
        canvas.SetActive(false);
        upperDetail.SetActive(false);
        generateDetailUpperRecycleImage.ClearExistingButtons();

        foreach (var pair in targetUIPairs)
        {
            if (pair.selectableObjectDetail.activeSelf)
            {
                pair.selectableObjectDetail.SetActive(false);
            }
        }

        isAnyObjectSelected = false;
        currentSelectedObject = null; // 清空當前選中的物件
    }
}
