using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;

public class RabbitGameManager : MonoBehaviour
{
    [Header("UI 設定")]
    public TextMeshProUGUI infoTextUI; // 顯示訊息的 UI

    [Header("按鈕物件")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton; // 提示按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable startButton; // 開始按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable endButton; // 結束按鈕

    [Header("遊戲物件")]
    public GameObject targetObject; // 開始後要顯示的物件
    public GameObject TakenBox; // 結束時出現的物件
    public GameObject GloveTaken; // 手套獲得的物件
    public GameObject animatedObject; // 觸發動畫的物件

    [Header("物品放置區域")]
    public ItemPlacementZone placementZone; // 連結到放置區

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor;
    private bool allItemsPlaced = false; // 追蹤是否所有物品都已放置

    private void Start()
    {
        // 監聽 Hover 事件
        hintButton.hoverEntered.AddListener(OnHoverEnter);
        hintButton.hoverExited.AddListener(OnHoverExit);

        startButton.hoverEntered.AddListener(OnHoverEnter);
        startButton.hoverExited.AddListener(OnHoverExit);

        endButton.hoverEntered.AddListener(OnHoverEnter);
        endButton.hoverExited.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
    }

    private void Update()
    {
        if (hoveringInteractor != null)
        {
            // 取得控制器輸入
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool isPressed = false;
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressed);

            if (isPressed)
            {
                if (hoveringInteractor.hasSelection) return;

                if (hoveringInteractor.interactablesHovered.Contains(hintButton))
                {
                    ShowHint();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(startButton))
                {
                    StartGame();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(endButton))
                {
                    EndGame();
                }
            }
        }
    }

    private void ShowHint()
    {
        UpdateText("「唉呀，真是糟透了！我剛剛弄丟了我的手套，" +
                   "但我太餓了，腦袋轉不過來。" +
                   "如果你能幫我找到冰淇淋、甜甜圈和漢堡，" +
                   "或許我就能想起來手套放在哪了。」");
    }

    private void StartGame()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogError("RabbitGameManager: targetObject 未設定！");
        }
    }

    public void OnAllItemsPlaced()
    {
        allItemsPlaced = true;
        Debug.Log("RabbitGameManager: 所有物品都已放置！");
    }

    private void EndGame()
    {
        if (allItemsPlaced) // 確保所有物品已放置
        {
            Debug.Log("所有物品收集完成，觸發結束動作！");
            DoAction();
        }
        else
        {
            UpdateText("「你還沒找到所有東西！請放置冰淇淋、甜甜圈和漢堡在旁邊黃色的盤子上！」");
        }
    }

    private void DoAction()
    {
        Debug.Log("RabbitGameManager: 開始結束流程...");

        // 更新 NPC 對話
        UpdateText("「哇！你真的找到所有東西了！" +
                   "冰淇淋、甜甜圈、漢堡，我現在感覺好多了！" +
                   "喔對了，我的手套......給你" +
                   "接下來你去找看看紅色的毛毛蟲吧");

        // 顯示獎勵物品
        TakenBox.SetActive(true);
        GloveTaken.SetActive(true);

        // 播放動畫
        if (animatedObject != null)
        {
            Animation animation = animatedObject.GetComponent<Animation>();
            if (animation != null)
            {
                animation.enabled = true;
                animation.Play();
                Invoke(nameof(HideObject), 15f);
            }
            else
            {
                Debug.LogError("RabbitGameManager: 動畫物件缺少 Animation 組件！");
            }
        }
        else
        {
            Debug.LogError("RabbitGameManager: animatedObject 未設定！");
        }
    }


    private void HideObject()
    {
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
            Debug.Log("RabbitGameManager: 動畫物件已隱藏");
        }
    }

    private void UpdateText(string message)
    {
        if (infoTextUI != null)
        {
            infoTextUI.text = message;
        }
        else
        {
            Debug.LogError("RabbitGameManager: infoTextUI 沒有設定！");
        }
    }
}
