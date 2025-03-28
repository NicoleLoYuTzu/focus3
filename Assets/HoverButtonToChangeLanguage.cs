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


  
    private void Start()
    {
      
        English.hoverEntered.AddListener(OnEndHoverEnter);
        English.hoverExited.AddListener(OnHoverExit);

        Chinese.hoverEntered.AddListener(OnEndHoverEnter);
        Chinese.hoverExited.AddListener(OnHoverExit);
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



                else if (hoveringInteractor.interactablesHovered.Contains(English))
                {
                    SetLanguageToEnglish();
                }
                else if (hoveringInteractor.interactablesHovered.Contains(Chinese))
                {
                    SetLanguageToChinese();
                }
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


    private void OnEndHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    // 離開 Hover
    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
    }

}

