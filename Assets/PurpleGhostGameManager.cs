using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PurpleGhostGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI infoTextUI; // 顯示文字的 UI

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton1; // 提示1按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton2; // 提示2按鈕

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor;

    private void Start()
    {
        // 監聽 Hover 進出事件
        hintButton1.hoverEntered.AddListener(OnHoverEnter);
        hintButton1.hoverExited.AddListener(OnHoverExit);

        hintButton2.hoverEntered.AddListener(OnHoverEnter);
        hintButton2.hoverExited.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hoveredObject = args.interactableObject as UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable;

        if (hoveredObject == hintButton1)
        {
            UpdateText("歡迎來到這片神秘的仙境，冒險者！\r\n你來得正是時候，我們正需要像你這樣的勇士。愛麗絲已經喝下了縮小藥水，現在被困在某個隱秘的角落裡。我們必須找到她，幫她恢復原來的大小。\r\n\r\n你的任務是完成所有挑戰，獲得至關重要的道具，最終找到放大藥水。只有這樣，才能幫助愛麗絲擺脫困境，恢復這片世界的秩序。");
        }
        else if (hoveredObject == hintButton2)
        {
            UpdateText("首先你需要找到一隻綠色的兔子並且與他對話!");
        }
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
