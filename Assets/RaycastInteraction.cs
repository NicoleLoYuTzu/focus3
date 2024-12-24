using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class RaycastInteractor : MonoBehaviour
{
    public GameObject canvas;  // 這裡應該是一個 GameObject，而非 Canvas 本身

    [System.Serializable]
    public class TargetUIPair
    {
        public GameObject selectableObject;      // 用來儲存可選物件的 GameObject
        public GameObject selectableObjectDetail; // 用來儲存對應的詳細物件（隱藏或顯示的部分）
    }

    private InputDevice leftHandDevice;  // 左手控制器的輸入設備
    private RaycastHit hitInfo;          // 儲存射線碰撞信息

    public List<TargetUIPair> targetUIPairs;  // 儲存 TargetUIPair 的列表

    private bool isAnyObjectSelected = false;  // 用來追蹤是否有物件被選中

    void Start()
    {
        // 獲取左手控制器的輸入設備
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Debug.Log("Start: Left hand device initialized");
    }

    void Update()
    {
        // 發射射線並檢查是否有按下板機（trigger）
        if (leftHandDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed) && isPressed)
        {
            Debug.Log("Update: Trigger pressed");

            // 發射射線，從左手控制器的位置開始，並且射向它的前方
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 如果射線碰撞到物件，印出物件的名字
                Debug.Log("Raycast hit object: " + hitInfo.collider.gameObject.name);

                bool foundSelectable = false;  // 標記是否找到對應的 selectableObject

                // 使用 foreach 檢查碰撞物件是否在可選物件列表中
                foreach (var pair in targetUIPairs)
                {
                    if (pair.selectableObject == hitInfo.collider.gameObject)
                    {
                        // 找到對應的可選物件，顯示對應的詳細物件
                        ShowSelectableObjectDetail(pair);
                        foundSelectable = true;
                        isAnyObjectSelected = true;  // 標記已選擇物件
                        break;  // 找到後立即跳出迴圈
                    }
                }

                if (!foundSelectable && isAnyObjectSelected)
                {
                    // 如果沒有選擇到可選物件，且之前有選擇物件，則關閉所有隱藏物件
                    DisableDetailObjects();
                    isAnyObjectSelected = false;  // 重置標記
                }
            }
            else
            {
                Debug.Log("No object hit by raycast");
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

        // 顯示對應的 selectableObjectDetail
        if (!selectedPair.selectableObjectDetail.activeSelf)
        {
            // 啟用該物件本身
            selectedPair.selectableObjectDetail.SetActive(true);
            Debug.Log("Enabling detail object: " + selectedPair.selectableObjectDetail.name);

            // 啟用該物件的所有子物件
            EnableAllChildObjects(selectedPair.selectableObjectDetail.transform);
        }
    }

    // 遞迴啟用所有子物件
    private void EnableAllChildObjects(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(true);  // 啟用子物件
            Debug.Log("Enabling child object: " + child.gameObject.name);

            // 如果子物件有子物件，遞迴呼叫
            EnableAllChildObjects(child);
        }
    }

    // 關閉所有隱藏物件及其子物件
    private void DisableDetailObjects()
    {
        Debug.Log("Disabling all detail objects");
        canvas.SetActive(false);

        foreach (var pair in targetUIPairs)
        {
            // 關閉所有 detail 物件
            if (pair.selectableObjectDetail.activeSelf)
            {
                pair.selectableObjectDetail.SetActive(false);
                Debug.Log("Disabling detail object: " + pair.selectableObjectDetail.name);
            }
        }
        isAnyObjectSelected = false;  // 重置標記
    }
}
