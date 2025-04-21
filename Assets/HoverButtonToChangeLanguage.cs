using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class HoverButtonToChangeLanguage : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 紀錄 Hover 的控制器
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable English;  // 愛心數量顯示
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable Chinese; // 結果顯示
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable startBtn; // 結果顯示

    public AudioSource audioSource;  // 音效播放元件
    public AudioClip btnClick1;     // 按下 hintButton1 時播放的音效
    public AudioClip btnClick2;     // 按下 hintButton2 時播放的音效
    public AudioClip goSound;     // 按下 hintButton2 時播放的音效

    private bool isSoundPlayed = false; // 是否播放過音效的標誌

    private void Start()
    {
      
        English.hoverEntered.AddListener(OnEndHoverEnter);
        English.hoverExited.AddListener(OnHoverExit);

        Chinese.hoverEntered.AddListener(OnEndHoverEnter);
        Chinese.hoverExited.AddListener(OnHoverExit);

        startBtn.hoverEntered.AddListener(OnEndHoverEnter);
        startBtn.hoverExited.AddListener(OnHoverExit);
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

            if ((isPressedLeft || isPressedRight) && !isSoundPlayed)
            {
                if (hoveringInteractor.hasSelection) return; // 避免重複觸發



                else if (hoveringInteractor.interactablesHovered.Contains(English))
                {
                    SetLanguageToEnglish();
                    PlaySound(btnClick1); // 播放 hintButton1 音效
                    isSoundPlayed = true;  // 標記音效已經播放
                }
                else if (hoveringInteractor.interactablesHovered.Contains(Chinese))
                {
                    SetLanguageToChinese();
                    PlaySound(btnClick2); // 播放 hintButton1 音效
                    isSoundPlayed = true;  // 標記音效已經播放
                }
                else if (hoveringInteractor.interactablesHovered.Contains(startBtn))
                {
                    StartTimerAction();
                    PlaySound(goSound); // 播放 hintButton1 音效
                    isSoundPlayed = true;  // 標記音效已經播放
                }
            }
        }
    }

    void StartTimerAction()
    {
        float startTime = Time.time;
        Debug.Log("StartTimerAction Start Time: " + startTime);
        PlayerPrefs.SetFloat("StartTime", startTime); // 保存数据
        PlayerPrefs.Save();  // 确保数据被保存
                             // 再次打印 PlayerPrefs 中的值，验证它是否保存成功
        float savedTime = PlayerPrefs.GetFloat("StartTime");
        Debug.Log("StartTimerAction Saved Time in PlayerPrefs: " + savedTime);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 播放指定音效
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


    private void OnEndHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
    }

    // 離開 Hover
    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
    }

}

