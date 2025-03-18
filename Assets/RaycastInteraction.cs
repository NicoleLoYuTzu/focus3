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

    [System.Obsolete]
    void Update()
    {

        if (!leftHandDevice.isValid)
        {
            TryGetLeftHandDevice(); // 確保 device 是有效的
        }
        bool isPressed;

        if (leftHandDevice.IsPressed(InputHelpers.Button.Trigger, out isPressed) && isPressed)
        {
            Debug.Log("Update: Trigger pressed");

            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, raycastLayerMask))
            {
                Debug.Log("Raycast hit object: " + hitInfo.collider.gameObject.name);

                foreach (var pair in targetUIPairs)
                {
                    if (pair.selectableObject == hitInfo.collider.gameObject)
                    {
                        isAnyObjectSelected = true; // 只有點到目標物才設為 true
                        break;
                    }
                }
            }

            else
            {
                Debug.Log("No object hit by raycast");

                if (isAnyObjectSelected)
                {
                    DisableDetailObjects();
                }
            }
        }
        else
        {
            Debug.Log("Update: Trigger not pressed");
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
