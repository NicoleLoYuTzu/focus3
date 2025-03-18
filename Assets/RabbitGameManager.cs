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

    [Header("必須收集的物品")]
    public List<GameObject> requiredItems = new List<GameObject>(); // 需要收集的物品
    private List<GameObject> collectedItems = new List<GameObject>(); // 已收集的物品

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor;

    private void Start()
    {
        // 監聽 Hover 進出事件
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
        if (hoveringInteractor != null) // 確保有 Hover 的控制器
        {
            // 取得控制器輸入
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool isPressed = false;
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressed);

            if (isPressed) // 當按下 Trigger
            {
                if (hoveringInteractor.hasSelection) return; // 避免重複觸發

                // 判斷是哪個按鈕被 Hover
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
        UpdateText("「唉呀，真是糟透了！我剛剛弄丟了我的手套，但沒辦法直接給你。" +
                   "你知道的，我現在實在太餓了，腦袋都轉不過來了。" +
                   "嗯...如果你能幫我找到我最愛的三樣東西——" +
                   "一個冰淇淋、甜甜圈，還有一個漢堡，我的肚子餵飽了，" +
                   "或許我就能想起來手套放在哪了。" +
                   "拜託了，我可沒時間耽誤，皇后的命令不容忽視啊！」");
    }

    private void StartGame()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // 顯示物件
        }
        else
        {
            Debug.LogError("RabbitGameActivate: targetObject 未設定！");
        }
    }

    private void EndGame()
    {
        if (HasCollectedAllItems()) // 檢查是否收集所有物品
        {
            Debug.Log("所有物品收集完成，觸發結束動作！");
            DoAction();
        }
        else
        {
            UpdateText("「你還沒找到所有東西！快點去收集冰淇淋、甜甜圈和漢堡！」");
        }
    }

    private bool HasCollectedAllItems()
    {
        foreach (GameObject item in requiredItems)
        {
            if (!collectedItems.Contains(item))
            {
                Debug.Log($"RabbitGameActivate: 缺少物品 - {item.name}");
                return false;
            }
        }
        return true;
    }

    private void DoAction()
    {
        Debug.Log("RabbitGameActivate: 開始結束流程...");
        TakenBox.SetActive(true);
        GloveTaken.SetActive(true);

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
                Debug.LogError("RabbitGameActivate: 動畫物件缺少 Animation 組件！");
            }
        }
        else
        {
            Debug.LogError("RabbitGameActivate: animatedObject 未設定！");
        }
    }

    private void HideObject()
    {
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
            Debug.Log("RabbitGameActivate: 動畫物件已隱藏");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"RabbitGameActivate: 進入觸發區 - {other.gameObject.name}");

        if (requiredItems.Contains(other.gameObject) && !collectedItems.Contains(other.gameObject))
        {
            collectedItems.Add(other.gameObject);
            Debug.Log($"RabbitGameActivate: 收集到 {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collectedItems.Contains(other.gameObject))
        {
            collectedItems.Remove(other.gameObject);
            Debug.Log($"RabbitGameActivate: {other.gameObject.name} 離開觸發區，移出已收集列表");
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
            Debug.LogError("RabbitGameActivate: infoTextUI 沒有設定！");
        }
    }
}
