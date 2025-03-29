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

    public AudioSource audioSource;  // 音效播放元件
    public AudioClip hintSound1;     // 按下 hintButton1 時播放的音效
    public AudioClip hintSound2;     // 按下 hintButton2 時播放的音效

    private bool isSoundPlayed = false; // 是否播放過音效的標誌

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

            if ((isPressedLeft || isPressedRight) && !isSoundPlayed) // 確保音效只播放一次
            {
                if (hoveringInteractor.interactablesHovered.Contains(hintButton1))
                {
                    UpdateText(LanguageManager.Instance.GetLocalizedString("PurpleGhostGreetingWords"));
                    PlaySound(hintSound1); // 播放 hintButton1 音效
                    isSoundPlayed = true;  // 標記音效已經播放
                }
                else if (hoveringInteractor.interactablesHovered.Contains(hintButton2))
                {
                    UpdateText(LanguageManager.Instance.GetLocalizedString("PurpleGhostHint"));
                    PlaySound(hintSound2); // 播放 hintButton2 音效
                    CustomRayInteractor.EndTask(purpleGhostEndTask);
                    isSoundPlayed = true;  // 標記音效已經播放
                }
            }
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        isSoundPlayed = false; // 重新進入 hover 區域時重置音效播放標誌
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 播放指定音效
        }
    }
}
