using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {

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
    }

    //private void Update()
    //{
    //    if (hoveringInteractor != null) // 確保目前有 Hover 的控制器
    //    {
    //        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    //        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    //        bool isPressedLeft = false, isPressedRight = false;

    //        InputHelpers.IsPressed(leftHandDevice, InputHelpers.Button.Trigger, out isPressedLeft);
    //        InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.Trigger, out isPressedRight);

    //        if (isPressedLeft || isPressedRight) // 任何一隻手的 Trigger 被按下
    //        {
    //            // 🔹 使用 interactablesSelected 檢查選中的物件
    //            if (hoveringInteractor.IsSelecting(startButton))
    //            {
    //                StartGame();
    //            }
    //            else if (hoveringInteractor.IsSelecting(endButton))
    //            {
    //                EndGame();
    //            }
    //        }
    //    }
    //}
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
                } else if (hoveringInteractor.interactablesHovered.Contains(hintButton)) {
                    GiveHint();
                }
            }
        }
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
    }

    // 當結束被觸發
    public void EndGame()
    {
        Debug.Log("End Button Pressed");
        wingUI.SetActive(false);
        heartUI.SetActive(false);

        if (wingCount == 3 && heartCount == 1)
        {
            resultText.text = "任務達成!";
            UpdateMaterials();
        }
        else
        {
            resultText.text = "任務未達成!";
        }
    }

    public void GiveHint() {
        resultText.text = "我的權杖不完整了！它原本擁有三對翅膀和一顆愛心，現在全都不見了！去，把它們找回來！不然……\r\n你就別想見到愛麗絲了！我可沒時間等太久，快去快回！還愣著幹什麼？快去工作！";
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
