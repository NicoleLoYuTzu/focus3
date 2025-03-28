using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 單例模式

    private int wingCount = 0;  // 翅膀數量
    private int heartCount = 0; // 愛心數量

    public TextMeshProUGUI wingText;   // 翅膀數量顯示
    public TextMeshProUGUI heartText;  // 愛心數量顯示
    public TextMeshProUGUI resultText; // 結果顯示

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton; // 開始按鈕 (XR 互動)
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable startButton; // 開始按鈕 (XR 互動)
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable endButton;   // 結束按鈕 (XR 互動)

    public GameObject wingUI;  // 翅膀 UI
    public GameObject heartUI; // 愛心 UI

    public Material wingMaterial;  // 正確的翅膀材質
    public Material heartMaterial; // 正確的愛心材質
    public GameObject[] objectsToUpdate; // 需要更新材質的 4 個物件

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 紀錄 Hover 的控制器
    public GameObject animatedObject;
    public GameObject magicWand;
    public CustomRayInteractor customRayInteractor;
    public GameObject princess;


    //public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable English;  // 愛心數量顯示
    //public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable Chinese; // 結果顯示


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        wingUI.SetActive(false);
        heartUI.SetActive(false);
        magicWand.SetActive(false);
        if (hintButton != null)
        {
            hintButton.hoverEntered.AddListener(OnStartHoverEnter);
            hintButton.hoverExited.AddListener(OnHoverExit);
        }
        // 綁定 Hover 事件到開始按鈕
        if (startButton != null)
        {
            startButton.hoverEntered.AddListener(OnStartHoverEnter);
            startButton.hoverExited.AddListener(OnHoverExit);
        }

        // 綁定 Hover 事件到結束按鈕
        if (endButton != null)
        {
            endButton.hoverEntered.AddListener(OnEndHoverEnter);
            endButton.hoverExited.AddListener(OnHoverExit);
        }

        //English.hoverEntered.AddListener(OnEndHoverEnter);
        //English.hoverExited.AddListener(OnHoverExit);

        //Chinese.hoverEntered.AddListener(OnEndHoverEnter);
        //Chinese.hoverExited.AddListener(OnHoverExit);
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


                //else if (hoveringInteractor.interactablesHovered.Contains(English))
                //{
                //    SetLanguageToEnglish();
                //}
                //else if (hoveringInteractor.interactablesHovered.Contains(Chinese))
                //{
                //    SetLanguageToChinese();
                //}
            }
        }
    }
    // 假設有一個語言選擇按鈕
    public void SetLanguageToEnglish()
    {
        PlayerPrefs.SetString("selectedLanguage", "en");  // 保存語言設定為英文
        PlayerPrefs.Save();  // 確保儲存設定
        Debug.Log("Language set to English: " + PlayerPrefs.GetString("selectedLanguage"));  // 顯示目前的語言設定

        LanguageManager.Instance.SetLanguage("en");  // 更新語言
    }

    public void SetLanguageToChinese()
    {
        PlayerPrefs.SetString("selectedLanguage", "zh-Hans");  // 保存語言設定為繁體中文
        PlayerPrefs.Save();  // 確保儲存設定
        Debug.Log("Language set to Chinese: " + PlayerPrefs.GetString("selectedLanguage"));  // 顯示目前的語言設定

        LanguageManager.Instance.SetLanguage("zh-Hans");  // 更新語言
    }



    // 進入 Hover (開始按鈕)
    private void OnStartHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    // 進入 Hover (結束按鈕)
    private void OnEndHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    // 離開 Hover
    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
    }

    // 當開始被觸發
    public void StartGame()
    {
        Debug.Log("Start Button Pressed");
        wingUI.SetActive(true);
        heartUI.SetActive(true);
        //customRayInteractor.StartTask(princess);

    }

    // 當結束被觸發
    public void EndGame()
    {
        Debug.Log("End Button Pressed");
        wingUI.SetActive(false);
        heartUI.SetActive(false);

        string resultMessage = (wingCount == 3 && heartCount == 1) ? LanguageManager.Instance.GetLocalizedString("MissionComplete") : LanguageManager.Instance.GetLocalizedString("MissionFailed");


        // 顯示結果
        resultText.text = resultMessage;

        // 執行任務達成時的操作
        if (wingCount == 3 && heartCount == 1)
        {
            UpdateMaterials();
            PlayAnimation(); // 播放動畫
            magicWand.SetActive(true);
            customRayInteractor.EndTask(princess);
        }
    }

    public void GiveHint()
    {
//        我的權杖不完整了！它原本擁有三對翅膀和一顆愛心，現在全都不見了！去，把它們找回來！不然……
//你就別想見到愛麗絲了！我可沒時間等太久，快去快回！還愣著幹什麼？快去工作！
        resultText.text = LanguageManager.Instance.GetLocalizedString("RedPrincessGreetingWord");
    }

    // 更新物件材質
    private void UpdateMaterials()
    {
        if (objectsToUpdate.Length >= 4)
        {
            objectsToUpdate[0].GetComponent<Renderer>().material = wingMaterial;
            objectsToUpdate[1].GetComponent<Renderer>().material = wingMaterial;
            objectsToUpdate[2].GetComponent<Renderer>().material = wingMaterial;
            objectsToUpdate[3].GetComponent<Renderer>().material = heartMaterial;
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



    // 收集物品
    public void CollectItem(string itemType)
    {
        if (itemType == "Wing" && wingCount < 3)
        {
            wingCount++;
        }
        else if (itemType == "Heart" && heartCount < 1)
        {
            heartCount = 1;
        }

        UpdateUI();
    }

    // 更新 UI 文字
    private void UpdateUI()
    {
        wingText.text = $"*{wingCount}";
        heartText.text = $"*{heartCount}";
    }
}
