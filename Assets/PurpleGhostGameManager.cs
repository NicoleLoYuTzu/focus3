using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PurpleGhostGameManager : MonoBehaviour
{
    public TextMeshProUGUI infoTextUI; // 顯示文字的 UI

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton1; // 提示1按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton2; // 提示2按鈕

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 當前 Hover 的控制器
    public CustomRayInteractor CustomRayInteractor;
    public GameObject purpleGhostEndTask;

    private void Start()
    {
        // 監聽 Hover 進出事件
        hintButton1.hoverEntered.AddListener(OnHoverEnter);
        hintButton1.hoverExited.AddListener(OnHoverExit);

        hintButton2.hoverEntered.AddListener(OnHoverEnter);
        hintButton2.hoverExited.AddListener(OnHoverExit);
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
                // 🔹 判斷 Hover 的物件來顯示不同的提示
                if (hoveringInteractor.interactablesHovered.Contains(hintButton1))
                {
                    UpdateText("歡迎來到這片神秘的仙境，冒險者！\r\n你來得正是時候，我們正需要像你這樣的勇士。愛麗絲已經喝下了縮小藥水，現在被困在某個隱秘的角落裡。我們必須找到她，幫她恢復原來的大小。\r\n\r\n你的任務是完成所有挑戰，獲得至關重要的道具，最終找到放大藥水。只有這樣，才能幫助愛麗絲擺脫困境，恢復這片世界的秩序。");
                }
                else if (hoveringInteractor.interactablesHovered.Contains(hintButton2))
                {
                    UpdateText("首先你需要找到一隻綠色的兔子並且與他對話!");
                    CustomRayInteractor.EndTask(purpleGhostEndTask);
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

    private void UpdateText(string message)
    {
        if (infoTextUI != null)
        {
            infoTextUI.text = message;
        }
        else
        {
            Debug.LogError("infoTextUI 沒有設定！");
        }
    }
}
