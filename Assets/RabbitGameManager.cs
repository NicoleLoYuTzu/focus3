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

    public CustomRayInteractor customRayInteractor;
    public GameObject rabbitGameEndTask;
    public AudioSource audioSource;  // 🔹 音效播放元件
    public AudioClip Clicked;     // 🔹 按下 hintButton2 時播放的音效
    public AudioClip GameStart;     // 🔹 按下 hintButton1 時播放的音效
    public AudioClip Success;     // 🔹 按下 hintButton1 時播放的音效
    public AudioClip Failed;     // 🔹 按下 hintButton2 時播放的音效

    private bool isSoundPlayed = false; // 是否播放過音效的標誌


    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 🔹 播放指定音效
        }
    }

    private void Start()
    {
        targetObject.SetActive(false); // 顯示目標物件
        GloveTaken.SetActive(false);
        animatedObject.SetActive(false);
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
        isSoundPlayed = false; // 重新進入 hover 區域時重置音效播放標誌
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (hoveringInteractor == args.interactorObject)
        {
            hoveringInteractor = null;
        }
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
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
        isSoundPlayed = true; // 標記音效已經播放
        PlaySound(Clicked);
        UpdateText(LanguageManager.Instance.GetLocalizedString("RabbitHint"));
        //UpdateText("「唉呀，真是糟透了！我剛剛弄丟了我的手套，" +
        //           "但我太餓了，腦袋轉不過來。" +
        //           "如果你能幫我找到冰淇淋、甜甜圈和漢堡，" +
        //           "或許我就能想起來手套放在哪了。」");
    }

    private void StartGame()
    {
        isSoundPlayed = true; // 標記音效已經播放
        // 檢查是否有 PurpleGhost 任務
        if (customRayInteractor.completedTasks.ContainsKey("PurpleGhost") && customRayInteractor.completedTasks["PurpleGhost"] == true)
        {
            // 如果 PurpleGhost 任務已經完成，顯示「任務開始」
            //UpdateText("任務開始！快開始找下一個物品吧！");
            UpdateText(LanguageManager.Instance.GetLocalizedString("RabbitGameStart"));
            PlaySound(GameStart);
            if (targetObject != null)
            {
                targetObject.SetActive(true); // 顯示目標物件
            }
            else
            {
                Debug.LogError("RabbitGameManager: targetObject 未設定！");
            }
        }
        else
        {

            PlaySound(Failed);
            // 如果 PurpleGhost 任務未完成，提示玩家去找紫色精靈
            //UpdateText("你應該先去找某位紫色的東東? 好像在某個角落");
            UpdateText(LanguageManager.Instance.GetLocalizedString("RabbitGameCannotStart"));
            
        }
    }

    public void OnAllItemsPlaced()
    {
        allItemsPlaced = true;
        Debug.Log("RabbitGameManager: 所有物品都已放置！");
    }

    private void EndGame()
    {
        isSoundPlayed = true; // 標記音效已經播放
        if (allItemsPlaced) // 確保所有物品已放置
        {
            Debug.Log("所有物品收集完成，觸發結束動作！");
            DoAction();

        }
        else
        {
            PlaySound(Failed);
            if (!customRayInteractor.completedTasks.ContainsKey("PurpleGhost"))
            {
                UpdateText(LanguageManager.Instance.GetLocalizedString("RabbitGameCannotStart"));
                
            }
            else
            {
                // 獲取未放置的物品列表
                List<GameObject> missingItems = placementZone.GetMissingItems();
                if (missingItems.Count > 0)
                {
                    //string missingItemsText = "「你還沒找到所有東西！請放置：";
                    string missingItemsText =LanguageManager.Instance.GetLocalizedString("RabbitGameFailed1");
                    foreach (var item in missingItems)
                    {
                        missingItemsText += item.name + " ";
                    }
                    //missingItemsText += "在旁邊黃色的盤子上！」";
                    missingItemsText += LanguageManager.Instance.GetLocalizedString("RabbitGameFailed2");
                    UpdateText(missingItemsText); // 顯示缺少物品的訊息
                }
            }

        }
    }


    private void DoAction()
    {
        Debug.Log("RabbitGameManager: 開始結束流程...");
        PlaySound(Success);
        // 更新 NPC 對話
        //UpdateText("「哇！你真的找到所有東西了！" +
        //           "冰淇淋、甜甜圈、漢堡，我現在感覺好多了！" +
        //           "喔對了，我的手套......給你" +
        //           "接下來你去找看看紅色的毛毛蟲吧");

        UpdateText(LanguageManager.Instance.GetLocalizedString("RabbitGameSuccess"));
        customRayInteractor.EndTask(rabbitGameEndTask);

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
