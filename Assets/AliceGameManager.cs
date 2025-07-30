using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class AliceGameManager : MonoBehaviour
{
    public GameObject potion; // 放大藥水物件
    public TextMeshProUGUI infoTextUI; // 顯示訊息的 UI
    public GameObject animatedObject;

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable hintButton; // 提示按鈕
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable endButton; // 結束按鈕

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor hoveringInteractor; // 紀錄 Hover 的控制器
    public AudioSource audioSource;  // 音效播放元件
    public AudioClip clicked;     // 按下 hintButton1 時播放的音效
    public AudioClip success;     // 按下 hintButton2 時播放的音效
    public AudioClip fail;     // 按下 hintButton2 時播放的音效

    private bool isSoundPlayed = false; // 是否播放過音效的標誌

    public float totalDistance = 0f;
    public int totalTeleportSteps = 0; // 新增：傳送次數


    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 播放指定音效
        }
    }

    private void Start()
    {


        // 綁定 Hover 事件到提示按鈕
        if (hintButton != null)
        {
            hintButton.hoverEntered.AddListener(OnHintHoverEnter);
            hintButton.hoverExited.AddListener(OnHoverExit);
        }
        // 綁定 Hover 事件到結束按鈕
        if (endButton != null)
        {
            endButton.hoverEntered.AddListener(OnEndHoverEnter);
            endButton.hoverExited.AddListener(OnHoverExit);
        }
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
                if (hoveringInteractor.hasSelection) return; // 避免重複觸發

                // 判斷 Hover 的物件來決定要執行哪個按鈕
                if (hoveringInteractor.interactablesHovered.Contains(endButton))
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

    private void OnHintHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (infoTextUI != null)
        {
            //infoTextUI.text = "為什麼我變得那麼小... 幫我想想辦法";
            infoTextUI.text = LanguageManager.Instance.GetLocalizedString("AliceGreetingWords");
        }
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
    }

    private void OnEndHoverEnter(HoverEnterEventArgs args)
    {
        hoveringInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (infoTextUI != null)
        {
            //infoTextUI.text = "你帶來藥水了嗎?";
            infoTextUI.text = LanguageManager.Instance.GetLocalizedString("AskingPotion");

        }
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        hoveringInteractor = null;
        isSoundPlayed = false; // 離開 hover 區域時重置音效播放標誌
    }

    private void GiveHint()
    {
        PlaySound(clicked); // 播放 hintButton1 音效
        isSoundPlayed = true;  // 標記音效已經播放
        if (infoTextUI != null)
        {
            //infoTextUI.text = "也許有什麼能讓我變回來...";
            infoTextUI.text = LanguageManager.Instance.GetLocalizedString("AliceAskingToTurnBack");

        }
    }

    private void EndGame()
    {
        float elapsedTime = EndTimerAction();
        Debug.Log("StartTimerAction EndTimerAction Elapsed Time: " + elapsedTime + " seconds");
        totalDistance = PlayerPrefs.GetFloat("totalDistance", 0f);  // 如果沒找到，預設為 0
        totalTeleportSteps = PlayerPrefs.GetInt("navigationCount", 0);  // 新增：取得步數
        Debug.Log($"OnTeleportEnd EndGameEndGame  | Total Distance: {totalDistance}");



        if (potion.activeSelf) // 確保藥水存在
        {
            if (infoTextUI != null)
            {
                infoTextUI.text = LanguageManager.Instance.GetLocalizedString("AliceDrinkPotion")
    + $"\nFinish time: {elapsedTime} seconds"
    + $"\nTotal Distance: {totalDistance}"
    + $"\nTotal Teleport Steps: {totalTeleportSteps}";
                // 將結果寫入 CSV
                CSVLogger.LogGameData(elapsedTime, totalDistance, totalTeleportSteps);

                PlaySound(success); // 播放 hintButton1 音效
                isSoundPlayed = true;  // 標記音效已經播放
            }

            StartCoroutine(ShowMessageThenGrow());
        }
        else
        {
            PlaySound(fail); // 播放 hintButton1 音效
            isSoundPlayed = true;  // 標記音效已經播放
            infoTextUI.text = LanguageManager.Instance.GetLocalizedString("AskingPotion"); // 使用字符串插值
        }
    }

    // 修改为返回 float 类型
    public float EndTimerAction()
    {
        float elapsedTime = 0;

        if (PlayerPrefs.HasKey("StartTime"))  // 检查数据是否存在
        {
            float startTime = PlayerPrefs.GetFloat("StartTime");  // 获取存储的时间
            elapsedTime = Time.time - startTime;  // 计算经过的时间
        }

        return elapsedTime;  // 返回经过的时间
    }


    private IEnumerator ShowMessageThenGrow()
    {
        yield return new WaitForSeconds(2f); // 等待 2 秒讓玩家看到訊息

        PlayAnimation();
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


}

