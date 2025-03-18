using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CardSoldierGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable startButton;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable endButton;

    public GameObject gameArea;
    public GameObject wandUI;
    public TextMeshProUGUI infoTextUI;

    public Text heartText;
    public Text diamondText;
    public Text clubText;
    public Text spadeText;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor;

    public GameObject potion;  // 🔹 藥水的 GameObject (需在 Unity 拖入)
    public GameObject animatedObject;
    public GameObject potionShowCorner;
    public GameObject wandCornerClose;
    public CustomRayInteractor customRayInteractor;
    public GameObject endGameCloseCardSoldierPreview;

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

    private void Update()
    {
        if (hoveringInteractor != null) // 確保目前有 Hover 的控制器
        {
            // 取得左右手裝置
            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            bool isPressedLeft = false, isPressedRight = false;

            // 偵測 Trigger 按鈕
            InputHelpers.IsPressed(leftHandDevice, InputHelpers.Button.Trigger, out isPressedLeft);
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressedRight);

            if (isPressedLeft || isPressedRight) // 任何一隻手的 Trigger 被按下
            {
                if (hoveringInteractor.hasSelection) return; // 避免重複觸發

                // 🔹 判斷 Hover 的物件來決定要執行哪個按鈕
                if (hoveringInteractor.interactablesHovered.Contains(startButton))
                {
                    StartGame();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(endButton))
                {
                    EndGame();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(hintButton))
                {
                    GiveHint();
                }
            }
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
    }

    private void GiveHint()
    {
        UpdateText("你帶來了權杖啊！\n\n勇敢的玩家，請尋找紅心、黑桃、梅花和菱形四種撲克牌圖案。\n" +
                   "每種圖案都有其獨特的魅力，正確的卡片將揭示通往放大藥水的秘密。\n" +
                   "找到每張牌後，依照順序將數字輸入控制台。\n" +
                   "一旦完成，你將獲得真正的放大藥水，幫助你的朋友愛麗絲！祝你好運！");
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        if (gameArea != null)
        {
            gameArea.SetActive(true); // 啟動遊戲區域
            UpdateText("任務開始 請把將正確答案輸入在後面的操控台");
        }
        wandCornerClose.SetActive(false);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");

        if (heartText != null && diamondText != null && clubText != null && spadeText != null)
        {
            string heartValue = heartText.text;
            string diamondValue = diamondText.text;
            string clubValue = clubText.text;
            string spadeValue = spadeText.text;

            Debug.Log($"Heart: {heartValue}, Diamond: {diamondValue}, Club: {clubValue}, Spade: {spadeValue}");

            if (heartValue == "1" && diamondValue == "12" && clubValue == "10" && spadeValue == "8")
            {
                UpdateText("恭喜你！你成功解開了撲克牌的謎題，獲得了真正的放大藥水！");
                potion.SetActive(true); // 讓藥水出現
                PlayAnimation(); // 播放動畫
                potionShowCorner.SetActive(true);
                customRayInteractor.EndTask(endGameCloseCardSoldierPreview);
            }
            else
            {
                UpdateText("你輸入的數字似乎不正確，再試試看吧！");
            }
        }
        else
        {
            Debug.LogError("One or more card number texts are not assigned!");
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
            Debug.LogError("CardSoldierGameManager: infoTextUI is not assigned!");
        }
    }


    private void PlayAnimation()
    {
        if (animatedObject != null)
        {
            Animation animation = animatedObject.GetComponent<Animation>();
            if (animation != null)
            {
                Debug.Log("GameManager: Animation component found on target object.");
                animation.enabled = true; // 啟用動畫元件
                Debug.Log("GameManager: Animation component enabled.");
                animation.Play(); // 播放動畫
                Debug.Log("GameManager: Playing animation.");
                Invoke(nameof(HideObject), 15f); // 15 秒後隱藏物件
            }
            else
            {
                Debug.LogError("GameManager: No Animation component found on animatedObject.");
            }
        }
        else
        {
            Debug.LogError("GameManager: animatedObject is not assigned.");
        }
    }

    private void HideObject()
    {
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
            Debug.Log("GameManager: animatedObject is now hidden.");
        }
    }

}
